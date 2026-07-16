import { normalize, joinFields, type EnvelopeField } from './envelope.js';
import { mod97, mod11Pzn } from './checkDigits.js';
import { IfaUdiFormatError, type ParsedUdi, type UdiDi, type UdiPi, type UdiScheme } from './types.js';
import { FORBIDDEN_LOT_SN_CHARS, ITEM_REFERENCE_CHARSET, ALPHANUMERIC_UPPER_CHARSET } from './validation.js';

export function check(barcode: string): boolean {
  try {
    parseUdi(barcode);
    return true;
  } catch (err) {
    if (err instanceof IfaUdiFormatError) {
      return false;
    }
    throw err;
  }
}

export function parseUdi(barcode: string): ParsedUdi {
  const { form, fields } = normalize(barcode);

  const udiDiFields = fields.filter((f) => f.di === '9N');
  if (udiDiFields.length !== 1) {
    throw new IfaUdiFormatError(
      `Expected exactly one UDI-DI (Data Identifier '9N') field, found ${udiDiFields.length}.`,
    );
  }

  const udiDi = parseUdiDi(udiDiFields[0].value);

  const piFields = fields.filter((f) => f.di !== '9N');
  const udiPi = parseUdiPi(piFields);
  udiPi.raw = joinFields(piFields, form);

  return { raw: barcode, udiDi, udiPi };
}

function parseUdiDi(value: string): UdiDi {
  if (value.startsWith('11')) {
    return parsePpn(value);
  }

  if (value.startsWith('13')) {
    return parseHpc(value);
  }

  if (value.startsWith('MA')) {
    return parseMasterUdiDi(value);
  }

  if (value.startsWith('15')) {
    return parseNationalCodeScheme(value, '15', 'AIC');
  }

  if (value.startsWith('17')) {
    return parseNationalCodeScheme(value, '17', 'AIM');
  }

  throw new IfaUdiFormatError(`UDI-DI value '${value}' does not start with a supported PRA-Code (11, 13, 15, 17, or MA).`);
}

function parsePpn(value: string): UdiDi {
  if (value.length !== 12) {
    throw new IfaUdiFormatError(`PPN value must be exactly 12 characters, got ${value.length}.`);
  }

  const pzn = value.slice(2, 10);
  const checkDigits = value.slice(10, 12);

  if (!/^[0-9]{8}$/.test(pzn)) {
    throw new IfaUdiFormatError(`PZN '${pzn}' must be 8 digits.`);
  }

  const expectedCheck = mod97(value.slice(0, 10));
  if (expectedCheck !== checkDigits) {
    throw new IfaUdiFormatError(`PPN check digits mismatch: expected '${expectedCheck}', got '${checkDigits}'.`);
  }

  const expectedPznCheck = mod11Pzn(pzn.slice(0, 7));
  if (expectedPznCheck !== pzn[7]) {
    throw new IfaUdiFormatError(`PZN check digit mismatch: expected '${expectedPznCheck}', got '${pzn[7]}'.`);
  }

  return {
    raw: value,
    scheme: 'PPN',
    praCode: '11',
    pzn,
    checkDigits,
  };
}

function parseHpc(value: string): UdiDi {
  if (value.length < 11 || value.length > 28) {
    throw new IfaUdiFormatError(`HPC value must be 11-28 characters, got ${value.length}.`);
  }

  const cin = value.slice(2, 7);
  const itemReferenceLength = value.length - 10;
  const itemReference = value.slice(7, 7 + itemReferenceLength);
  const pli = value[7 + itemReferenceLength];
  const checkDigits = value.slice(8 + itemReferenceLength, 10 + itemReferenceLength);

  if (!ALPHANUMERIC_UPPER_CHARSET.test(cin)) {
    throw new IfaUdiFormatError(`HPC CIN '${cin}' must be alphanumeric uppercase.`);
  }

  if (!ITEM_REFERENCE_CHARSET.test(itemReference)) {
    throw new IfaUdiFormatError(`HPC item reference '${itemReference}' contains characters outside 0-9, A-Z, '.', '-'.`);
  }

  if (pli < '0' || pli > '8') {
    throw new IfaUdiFormatError(
      `HPC packaging level index '${pli}' is invalid; only 0-8 are valid for UDI (9 is reserved for variable quantities).`,
    );
  }

  const expectedCheck = mod97(value.slice(0, -2));
  if (expectedCheck !== checkDigits) {
    throw new IfaUdiFormatError(`HPC check digits mismatch: expected '${expectedCheck}', got '${checkDigits}'.`);
  }

  return {
    raw: value,
    scheme: 'HPC',
    praCode: '13',
    cin,
    itemReference,
    packagingLevelIndex: pli.charCodeAt(0) - 48,
    checkDigits,
  };
}

function parseMasterUdiDi(value: string): UdiDi {
  if (value.length < 10 || value.length > 28) {
    throw new IfaUdiFormatError(`Master UDI-DI value must be 10-28 characters, got ${value.length}.`);
  }

  const cin = value.slice(2, 7);
  const deviceGroupLength = value.length - 9;
  const deviceGroupCode = value.slice(7, 7 + deviceGroupLength);
  const checkDigits = value.slice(7 + deviceGroupLength, 9 + deviceGroupLength);

  if (!ALPHANUMERIC_UPPER_CHARSET.test(cin)) {
    throw new IfaUdiFormatError(`Master UDI-DI CIN '${cin}' must be alphanumeric uppercase.`);
  }

  if (!ITEM_REFERENCE_CHARSET.test(deviceGroupCode)) {
    throw new IfaUdiFormatError(
      `Master UDI-DI device group code '${deviceGroupCode}' contains characters outside 0-9, A-Z, '.', '-'.`,
    );
  }

  const expectedCheck = mod97(value.slice(0, -2));
  if (expectedCheck !== checkDigits) {
    throw new IfaUdiFormatError(`Master UDI-DI check digits mismatch: expected '${expectedCheck}', got '${checkDigits}'.`);
  }

  return {
    raw: value,
    scheme: 'MASTER_UDI_DI',
    praCode: 'MA',
    cin,
    deviceGroupCode,
    checkDigits,
  };
}

function parseNationalCodeScheme(value: string, praCode: string, scheme: UdiScheme): UdiDi {
  if (value.length < 5 || value.length > 22) {
    throw new IfaUdiFormatError(`${scheme} value must be 5-22 characters, got ${value.length}.`);
  }

  const nationalCode = value.slice(2, -2);
  const checkDigits = value.slice(-2);

  if (!ITEM_REFERENCE_CHARSET.test(nationalCode)) {
    throw new IfaUdiFormatError(`${scheme} national code '${nationalCode}' contains characters outside 0-9, A-Z, '.', '-'.`);
  }

  const expectedCheck = mod97(value.slice(0, -2));
  if (expectedCheck !== checkDigits) {
    throw new IfaUdiFormatError(`${scheme} check digits mismatch: expected '${expectedCheck}', got '${checkDigits}'.`);
  }

  return {
    raw: value,
    scheme,
    praCode,
    nationalCode,
    checkDigits,
  };
}

function parseUdiPi(fields: EnvelopeField[]): UdiPi {
  const result: UdiPi = { raw: '' };
  let additionalGtins: string[] | undefined;

  for (const { di, value } of fields) {
    switch (di) {
      case '1T':
        result.lot = validateLotOrSerial(value, 'LOT');
        break;
      case 'D':
        result.expiryDate = parseDate(value, false);
        break;
      case '16D':
        result.manufacturingDate = parseDate(value, true);
        break;
      case 'S':
        result.serialNumber = validateLotOrSerial(value, 'SN');
        break;
      case 'Q':
        if (value.length < 1 || value.length > 8 || !/^[0-9]+$/.test(value)) {
          throw new IfaUdiFormatError(`Quantity '${value}' must be 1-8 digits.`);
        }
        result.quantity = Number.parseInt(value, 10);
        break;
      case '27Q':
        if (value.length < 4 || value.length > 20 || !/^[0-9.]+$/.test(value)) {
          throw new IfaUdiFormatError(`Price '${value}' must be 4-20 characters of digits and '.'.`);
        }
        result.price = value;
        break;
      case '33L':
        result.url = value;
        break;
      case '8P':
        if (value.length !== 14 || !/^[0-9]{14}$/.test(value)) {
          throw new IfaUdiFormatError(`Additional GTIN/NTIN '${value}' must be exactly 14 digits.`);
        }
        additionalGtins = additionalGtins ?? [];
        additionalGtins.push(value);
        break;
      default:
        throw new IfaUdiFormatError(`Unrecognized UDI-PI Data Identifier '${di}'.`);
    }
  }

  if (additionalGtins) {
    result.additionalGtins = additionalGtins;
  }

  return result;
}

function validateLotOrSerial(value: string, fieldName: string): string {
  if (value.length < 1 || value.length > 20) {
    throw new IfaUdiFormatError(`${fieldName} '${value}' must be 1-20 characters.`);
  }

  if (FORBIDDEN_LOT_SN_CHARS.test(value)) {
    throw new IfaUdiFormatError(`${fieldName} '${value}' contains a technically excluded character.`);
  }

  return value;
}

function parseDate(value: string, expectFourDigitYear: boolean): string {
  const expectedLength = expectFourDigitYear ? 8 : 6;
  if (value.length !== expectedLength || !/^[0-9]+$/.test(value)) {
    throw new IfaUdiFormatError(
      `Date '${value}' must be exactly ${expectedLength} digits (${expectFourDigitYear ? 'YYYYMMDD' : 'YYMMDD'}).`,
    );
  }

  const year = expectFourDigitYear ? Number.parseInt(value.slice(0, 4), 10) : 2000 + Number.parseInt(value.slice(0, 2), 10);
  const month = Number.parseInt(value.slice(expectFourDigitYear ? 4 : 2, expectFourDigitYear ? 6 : 4), 10);
  const day = Number.parseInt(value.slice(expectFourDigitYear ? 6 : 4, expectFourDigitYear ? 8 : 6), 10);

  if (month < 1 || month > 12) {
    throw new IfaUdiFormatError(`Date '${value}' has invalid month ${String(month).padStart(2, '0')}.`);
  }

  const yearStr = String(year).padStart(4, '0');
  const monthStr = String(month).padStart(2, '0');

  if (day === 0 && !expectFourDigitYear) {
    return `${yearStr}-${monthStr}`;
  }

  if (day < 1 || day > 31) {
    throw new IfaUdiFormatError(`Date '${value}' has invalid day ${String(day).padStart(2, '0')}.`);
  }

  return `${yearStr}-${monthStr}-${String(day).padStart(2, '0')}`;
}
