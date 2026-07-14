import { IfaUdiFormatError } from './types.js';

/**
 * Check-digit algorithms per documentation/IFA_UDI_Parser_Analysis.md section 5.
 */

/**
 * Modulo-97 check digit shared by PPN, HPC, Master UDI-DI and Basic UDI-DI.
 * `payload` must be the full value including the PRA-Code/IAC prefix but excluding the
 * trailing 2 check-digit characters.
 */
export function mod97(payload: string): string {
  let sum = 0;
  let weight = 2;
  for (let i = 0; i < payload.length; i++) {
    sum += payload.charCodeAt(i) * weight;
    weight++;
  }

  const remainder = sum % 97;
  return remainder.toString().padStart(2, '0');
}

/**
 * Modulo-11 check digit for the 7-digit PZN base. Throws if the computed remainder is 10,
 * which per spec is never issued as a PZN.
 */
export function mod11Pzn(sevenDigitBase: string): string {
  if (sevenDigitBase.length !== 7 || !/^[0-9]{7}$/.test(sevenDigitBase)) {
    throw new IfaUdiFormatError(`PZN base must be exactly 7 digits, got '${sevenDigitBase}'.`);
  }

  let sum = 0;
  for (let i = 0; i < 7; i++) {
    const digit = sevenDigitBase.charCodeAt(i) - 48;
    sum += digit * (i + 1);
  }

  const remainder = sum % 11;
  if (remainder === 10) {
    throw new IfaUdiFormatError(
      'Computed PZN check digit remainder is 10; this digit sequence was never issued as a PZN.',
    );
  }

  return String(remainder);
}
