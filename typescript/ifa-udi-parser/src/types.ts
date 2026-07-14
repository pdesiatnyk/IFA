export type UdiScheme = 'PPN' | 'HPC' | 'MASTER_UDI_DI';

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
