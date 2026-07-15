export type UdiScheme = 'PPN' | 'HPC' | 'MASTER_UDI_DI' | 'AIC' | 'AIM';

export interface UdiDi {
  raw: string;
  scheme: UdiScheme;
  praCode: string;
  checkDigits: string;

  /** PPN only. */
  pzn?: string;

  /** HPC and Master UDI-DI only. */
  cin?: string;

  /** HPC only. */
  itemReference?: string;

  /** HPC only. 0-8. */
  packagingLevelIndex?: number;

  /** Master UDI-DI only. */
  deviceGroupCode?: string;

  /**
   * AIC and AIM only. Opaque national code (Italy AIC / Portugal AIM) -- IFA does not publish
   * a length/charset/check-digit spec for this code, so only overall bounds (1-18 chars,
   * 0-9A-Z.-) plus the outer Mod-97 checksum are validated.
   */
  nationalCode?: string;
}

export interface UdiPi {
  lot?: string;
  /** Formatted as YYYY-MM-DD, or YYYY-MM when the source day is "00" (unspecified). */
  expiryDate?: string;
  /** Formatted as YYYY-MM-DD. */
  manufacturingDate?: string;
  serialNumber?: string;
  quantity?: number;
  price?: string;
  url?: string;
  additionalGtins?: string[];
}

export interface ParsedUdi {
  udiDi: UdiDi;
  udiPi: UdiPi;
}

export class IfaUdiFormatError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'IfaUdiFormatError';
  }
}

export type BuildUdiDiInput =
  | { scheme: 'PPN'; pznBase: string }
  | { scheme: 'HPC'; cin: string; itemReference: string; packagingLevelIndex: number }
  | { scheme: 'MASTER_UDI_DI'; cin: string; deviceGroupCode: string }
  | { scheme: 'AIC'; nationalCode: string }
  | { scheme: 'AIM'; nationalCode: string };

export interface BuildUdiPiInput {
  lot?: string;
  /** "YYYY-MM-DD", or "YYYY-MM" for an unspecified day (encodes as day "00"). */
  expiryDate?: string;
  /** "YYYY-MM-DD" only (MFD has no month-only form). */
  manufacturingDate?: string;
  serialNumber?: string;
  quantity?: number;
  price?: string;
  url?: string;
  additionalGtins?: string[];
}

export interface BuildUdiInput {
  udiDi: BuildUdiDiInput;
  udiPi?: BuildUdiPiInput;
}

export type EnvelopeForm = 'interpretationLine' | 'rawIso15434' | 'din16598';

export class IfaUdiBuildError extends Error {
  constructor(
    message: string,
    public readonly field: string,
    public readonly reason: string,
  ) {
    super(`${message} (field ${field})`);
    this.name = 'IfaUdiBuildError';
  }
}
