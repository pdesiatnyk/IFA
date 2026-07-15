import { serialize, type EnvelopeField } from './envelope.js';
import { mod97, mod11Pzn } from './checkDigits.js';
import { ALPHANUMERIC_UPPER_CHARSET, ITEM_REFERENCE_CHARSET, FORBIDDEN_LOT_SN_CHARS } from './validation.js';
import {
  IfaUdiBuildError,
  IfaUdiFormatError,
  type BuildUdiInput,
  type BuildUdiPiInput,
  type EnvelopeForm,
} from './types.js';

const DATE_ONLY_PATTERN = /^([0-9]{4})-([0-9]{2})-([0-9]{2})$/;
const YEAR_MONTH_PATTERN = /^([0-9]{4})-([0-9]{2})$/;

/**
 * Constructs a valid IFA UDI barcode string from structured UDI-DI/UDI-PI input, the inverse of
 * {@link parseUdi}. Check digits are always computed, never user-supplied. Throws
 * {@link IfaUdiBuildError} on invalid input.
 */
export function buildUdi(input: BuildUdiInput, envelopeForm: EnvelopeForm = 'interpretationLine'): string {
  if (envelopeForm === 'din16598' && input.udiDi.scheme !== 'HPC') {
    throw new IfaUdiBuildError('DIN 16598 envelope form is only valid for the HPC scheme.', 'envelopeForm', 'DIN16598_HPC_ONLY');
  }

  const fields: EnvelopeField[] = [{ di: '9N', value: buildUdiDi(input.udiDi) }, ...buildUdiPiFields(input.udiPi)];
  return serialize(fields, envelopeForm);
}

function buildUdiDi(input: BuildUdiInput['udiDi']): string {
  switch (input.scheme) {
    case 'PPN':
      return buildPpn(input.pznBase);
    case 'HPC':
      return buildHpc(input.cin, input.itemReference, input.packagingLevelIndex);
    case 'MASTER_UDI_DI':
      return buildMasterUdiDi(input.cin, input.deviceGroupCode);
    case 'AIC':
      return buildNationalCodeScheme('15', input.nationalCode);
    case 'AIM':
      return buildNationalCodeScheme('17', input.nationalCode);
  }
}

function buildPpn(pznBase: string): string {
  let pznCheckDigit: string;
  try {
    pznCheckDigit = mod11Pzn(pznBase);
  } catch (err) {
    if (err instanceof IfaUdiFormatError) {
      throw new IfaUdiBuildError(err.message, 'udiDi.pznBase', 'INVALID_PZN_BASE');
    }
    throw err;
  }

  const value = `11${pznBase}${pznCheckDigit}`; // "11" + 7-digit base + 1-digit check = 10 chars
  return value + mod97(value);
}

function buildHpc(cin: string, itemReference: string, packagingLevelIndex: number): string {
  assertCin(cin, 'udiDi.cin');

  if (itemReference.length < 1 || itemReference.length > 18 || !ITEM_REFERENCE_CHARSET.test(itemReference)) {
    throw new IfaUdiBuildError('Must be 1-18 characters of 0-9, A-Z, \'.\' or \'-\'.', 'udiDi.itemReference', 'INVALID_ITEM_REFERENCE');
  }

  if (!Number.isInteger(packagingLevelIndex) || packagingLevelIndex < 0 || packagingLevelIndex > 8) {
    throw new IfaUdiBuildError(
      'Must be an integer 0-8 (9 is reserved for variable quantities, invalid for UDI).',
      'udiDi.packagingLevelIndex',
      'INVALID_PACKAGING_LEVEL_INDEX',
    );
  }

  const value = `13${cin}${itemReference}${packagingLevelIndex}`;
  return value + mod97(value);
}

function buildMasterUdiDi(cin: string, deviceGroupCode: string): string {
  assertCin(cin, 'udiDi.cin');

  if (deviceGroupCode.length < 1 || deviceGroupCode.length > 19 || !ITEM_REFERENCE_CHARSET.test(deviceGroupCode)) {
    throw new IfaUdiBuildError(
      'Must be 1-19 characters of 0-9, A-Z, \'.\' or \'-\'.',
      'udiDi.deviceGroupCode',
      'INVALID_DEVICE_GROUP_CODE',
    );
  }

  const value = `MA${cin}${deviceGroupCode}`;
  return value + mod97(value);
}

function buildNationalCodeScheme(praCode: string, nationalCode: string): string {
  if (nationalCode.length < 1 || nationalCode.length > 18 || !ITEM_REFERENCE_CHARSET.test(nationalCode)) {
    throw new IfaUdiBuildError('Must be 1-18 characters of 0-9, A-Z, \'.\' or \'-\'.', 'udiDi.nationalCode', 'INVALID_NATIONAL_CODE');
  }

  const value = `${praCode}${nationalCode}`;
  return value + mod97(value);
}

function assertCin(cin: string, field: string): void {
  if (cin.length !== 5 || !ALPHANUMERIC_UPPER_CHARSET.test(cin)) {
    throw new IfaUdiBuildError('Must be exactly 5 alphanumeric uppercase characters.', field, 'INVALID_CIN');
  }
}

function buildUdiPiFields(pi: BuildUdiPiInput | undefined): EnvelopeField[] {
  if (!pi) {
    return [];
  }

  const fields: EnvelopeField[] = [];
  if (pi.lot !== undefined) {
    fields.push({ di: '1T', value: assertLotOrSerial(pi.lot, 'udiPi.lot') });
  }
  if (pi.expiryDate !== undefined) {
    fields.push({ di: 'D', value: encodeExpiryDate(pi.expiryDate) });
  }
  if (pi.manufacturingDate !== undefined) {
    fields.push({ di: '16D', value: encodeManufacturingDate(pi.manufacturingDate) });
  }
  if (pi.serialNumber !== undefined) {
    fields.push({ di: 'S', value: assertLotOrSerial(pi.serialNumber, 'udiPi.serialNumber') });
  }
  if (pi.quantity !== undefined) {
    fields.push({ di: 'Q', value: encodeQuantity(pi.quantity) });
  }
  if (pi.price !== undefined) {
    fields.push({ di: '27Q', value: assertPrice(pi.price) });
  }
  if (pi.url !== undefined) {
    fields.push({ di: '33L', value: pi.url });
  }
  for (const gtin of pi.additionalGtins ?? []) {
    fields.push({ di: '8P', value: assertGtin(gtin) });
  }

  return fields;
}

function assertLotOrSerial(value: string, field: string): string {
  if (value.length < 1 || value.length > 20) {
    throw new IfaUdiBuildError('Must be 1-20 characters.', field, 'INVALID_LOT_OR_SERIAL');
  }

  if (FORBIDDEN_LOT_SN_CHARS.test(value)) {
    throw new IfaUdiBuildError('Contains a technically excluded character.', field, 'INVALID_LOT_OR_SERIAL');
  }

  return value;
}

function encodeExpiryDate(value: string): string {
  const dateOnly = DATE_ONLY_PATTERN.exec(value);
  if (dateOnly) {
    const [, year, month, day] = dateOnly;
    assertMonthDay(month, day, 'udiPi.expiryDate');
    return `${year.slice(2)}${month}${day}`;
  }

  const yearMonth = YEAR_MONTH_PATTERN.exec(value);
  if (yearMonth) {
    const [, year, month] = yearMonth;
    assertMonthDay(month, '01', 'udiPi.expiryDate');
    return `${year.slice(2)}${month}00`;
  }

  throw new IfaUdiBuildError('Must be "YYYY-MM-DD" or "YYYY-MM".', 'udiPi.expiryDate', 'INVALID_DATE');
}

function encodeManufacturingDate(value: string): string {
  const dateOnly = DATE_ONLY_PATTERN.exec(value);
  if (!dateOnly) {
    throw new IfaUdiBuildError('Must be "YYYY-MM-DD".', 'udiPi.manufacturingDate', 'INVALID_DATE');
  }

  const [, year, month, day] = dateOnly;
  assertMonthDay(month, day, 'udiPi.manufacturingDate');
  return `${year}${month}${day}`;
}

function assertMonthDay(month: string, day: string, field: string): void {
  const monthNum = Number.parseInt(month, 10);
  const dayNum = Number.parseInt(day, 10);
  if (monthNum < 1 || monthNum > 12) {
    throw new IfaUdiBuildError(`Invalid month ${month}.`, field, 'MONTH_OUT_OF_RANGE');
  }
  if (dayNum < 1 || dayNum > 31) {
    throw new IfaUdiBuildError(`Invalid day ${day}.`, field, 'DAY_OUT_OF_RANGE');
  }
}

function encodeQuantity(quantity: number): string {
  if (!Number.isInteger(quantity) || quantity < 0) {
    throw new IfaUdiBuildError('Must be a non-negative integer.', 'udiPi.quantity', 'INVALID_QUANTITY');
  }

  const value = String(quantity);
  if (value.length < 1 || value.length > 8) {
    throw new IfaUdiBuildError('Must be representable in 1-8 digits.', 'udiPi.quantity', 'INVALID_QUANTITY');
  }

  return value;
}

function assertPrice(value: string): string {
  if (value.length < 4 || value.length > 20 || !/^[0-9.]+$/.test(value)) {
    throw new IfaUdiBuildError('Must be 4-20 characters of digits and \'.\'.', 'udiPi.price', 'INVALID_PRICE');
  }

  return value;
}

function assertGtin(value: string): string {
  if (value.length !== 14 || !/^[0-9]{14}$/.test(value)) {
    throw new IfaUdiBuildError('Must be exactly 14 digits.', 'udiPi.additionalGtins', 'INVALID_GTIN');
  }

  return value;
}
