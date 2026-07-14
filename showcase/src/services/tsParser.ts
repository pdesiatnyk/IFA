import { parseUdi, IfaUdiFormatError } from 'ifa-udi-parser';
import type { ParseOutcome } from '../types.js';

export function parseWithTs(barcode: string): ParseOutcome {
  try {
    const result = parseUdi(barcode);
    return { success: true, result };
  } catch (err) {
    if (err instanceof IfaUdiFormatError) {
      return { success: false, error: { message: err.message } };
    }
    throw err;
  }
}
