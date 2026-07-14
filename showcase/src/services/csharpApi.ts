import type { BuildUdiInput, EnvelopeForm } from 'ifa-udi-parser';
import type { ParseOutcome, BuildOutcome } from '../types.js';

export async function parseWithCSharp(barcode: string): Promise<ParseOutcome> {
  const res = await fetch('/api/parse', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ barcode }),
  });
  return (await res.json()) as ParseOutcome;
}

export async function buildWithCSharp(input: BuildUdiInput, envelopeForm?: EnvelopeForm): Promise<BuildOutcome> {
  const res = await fetch('/api/build', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ input, envelopeForm }),
  });
  return (await res.json()) as BuildOutcome;
}
