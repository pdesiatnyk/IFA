import { buildUdi, IfaUdiBuildError } from 'ifa-udi-parser';
import type { BuildUdiInput, EnvelopeForm } from 'ifa-udi-parser';
import type { BuildOutcome } from '../types.js';

export function buildWithTs(input: BuildUdiInput, envelopeForm?: EnvelopeForm): BuildOutcome {
  try {
    const barcode = buildUdi(input, envelopeForm);
    return { success: true, barcode };
  } catch (err) {
    if (err instanceof IfaUdiBuildError) {
      return { success: false, error: { message: err.message, field: err.field, reason: err.reason } };
    }
    throw err;
  }
}
