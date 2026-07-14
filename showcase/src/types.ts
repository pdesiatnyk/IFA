import type { ParsedUdi } from 'ifa-udi-parser';

export interface ParseSuccess {
  success: true;
  result: ParsedUdi;
}
export interface ParseFailure {
  success: false;
  error: { message: string };
}
export type ParseOutcome = ParseSuccess | ParseFailure | { loading: true };

export interface BuildSuccess {
  success: true;
  barcode: string;
}
export interface BuildFailure {
  success: false;
  error: { message: string; field: string; reason: string };
}
export type BuildOutcome = BuildSuccess | BuildFailure | { loading: true };
