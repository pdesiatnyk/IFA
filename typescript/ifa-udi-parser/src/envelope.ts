import { IfaUdiFormatError, type EnvelopeForm } from './types.js';

/**
 * Detects and normalizes the three accepted IFA UDI barcode envelope forms (raw ISO/IEC 15434
 * Format 06, DIN 16598 keyboard-compatible, and the printable Interpretation Line) into an
 * ordered list of (DataIdentifier, Value) pairs. See documentation/IFA_UDI_Parser_Analysis.md
 * section 4.
 */

const RECORD_SEPARATOR = String.fromCharCode(0x1e);
const GROUP_SEPARATOR = String.fromCharCode(0x1d);
const END_OF_TRANSMISSION = String.fromCharCode(0x04);

// Known Data Identifiers, longest first so prefix matching is unambiguous.
const KNOWN_DATA_IDENTIFIERS = ['27Q', '16D', '33L', '9N', '1T', '8P', 'D', 'S', 'Q'];

const INTERPRETATION_LINE_FIELD_PATTERN = /\(([0-9A-Za-z]+)\)([^()]*)/g;

export interface EnvelopeField {
  di: string;
  value: string;
}

export function normalize(barcode: string): EnvelopeField[] {
  if (!barcode) {
    throw new IfaUdiFormatError('Barcode input must not be empty.');
  }

  if (barcode.startsWith('[)>')) {
    return parseRawIso15434(barcode);
  }

  if (barcode.startsWith('.')) {
    return parseDin16598(barcode);
  }

  if (barcode.startsWith('(') && new RegExp(INTERPRETATION_LINE_FIELD_PATTERN).test(barcode)) {
    return parseInterpretationLine(barcode);
  }

  throw new IfaUdiFormatError(
    'Barcode does not match any recognized IFA UDI envelope form (raw ISO/IEC 15434, DIN 16598, or Interpretation Line).',
  );
}

function parseRawIso15434(barcode: string): EnvelopeField[] {
  let rest = barcode.slice(3); // strip "[)>"

  if (rest.startsWith(RECORD_SEPARATOR)) {
    rest = rest.slice(1);
  }

  if (!rest.startsWith('06')) {
    throw new IfaUdiFormatError('Raw ISO/IEC 15434 envelope must use Format 06.');
  }

  rest = rest.slice(2);

  if (rest.startsWith(GROUP_SEPARATOR)) {
    rest = rest.slice(1);
  }

  while (rest.endsWith(RECORD_SEPARATOR) || rest.endsWith(END_OF_TRANSMISSION)) {
    rest = rest.slice(0, -1);
  }

  const rawFields = rest.split(GROUP_SEPARATOR);
  return splitDataIdentifiers(rawFields);
}

function parseDin16598(barcode: string): EnvelopeField[] {
  const rest = barcode.slice(1); // strip leading '.'
  const rawFields = rest.split('^');
  return splitDataIdentifiers(rawFields);
}

function parseInterpretationLine(barcode: string): EnvelopeField[] {
  const result: EnvelopeField[] = [];
  const pattern = new RegExp(INTERPRETATION_LINE_FIELD_PATTERN);
  let match: RegExpExecArray | null;
  while ((match = pattern.exec(barcode)) !== null) {
    result.push({ di: match[1], value: match[2] });
  }

  return result;
}

export function serialize(fields: EnvelopeField[], form: EnvelopeForm): string {
  switch (form) {
    case 'interpretationLine':
      return fields.map((f) => `(${f.di})${f.value}`).join('');
    case 'rawIso15434':
      return (
        `[)>${RECORD_SEPARATOR}06${GROUP_SEPARATOR}` +
        fields.map((f) => `${f.di}${f.value}`).join(GROUP_SEPARATOR) +
        `${RECORD_SEPARATOR}${END_OF_TRANSMISSION}`
      );
    case 'din16598':
      return '.' + fields.map((f) => `${f.di}${f.value}`).join('^');
  }
}

function splitDataIdentifiers(rawFields: string[]): EnvelopeField[] {
  return rawFields.map((field) => {
    const di = KNOWN_DATA_IDENTIFIERS.find((candidate) => field.startsWith(candidate));
    if (di === undefined) {
      throw new IfaUdiFormatError(`Field '${field}' does not start with a recognized Data Identifier.`);
    }

    return { di, value: field.slice(di.length) };
  });
}
