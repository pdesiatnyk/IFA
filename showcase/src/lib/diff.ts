export type FlatValue = string | number | boolean | null | undefined;

export function flatten(obj: unknown, prefix = ''): Record<string, FlatValue> {
  const result: Record<string, FlatValue> = {};

  if (obj === null || obj === undefined) {
    if (prefix) {
      result[prefix] = obj as FlatValue;
    }
    return result;
  }

  if (Array.isArray(obj)) {
    if (obj.length === 0) {
      result[prefix] = '[]';
    } else {
      obj.forEach((item, i) => Object.assign(result, flatten(item, prefix ? `${prefix}[${i}]` : `[${i}]`)));
    }
    return result;
  }

  if (typeof obj === 'object') {
    const entries = Object.entries(obj as Record<string, unknown>);
    if (entries.length === 0) {
      if (prefix) {
        result[prefix] = '{}';
      }
      return result;
    }
    for (const [key, value] of entries) {
      Object.assign(result, flatten(value, prefix ? `${prefix}.${key}` : key));
    }
    return result;
  }

  result[prefix] = obj as FlatValue;
  return result;
}

export interface DiffRow {
  path: string;
  left: FlatValue;
  right: FlatValue;
  mismatch: boolean;
}

export function diffRows(left: unknown, right: unknown): DiffRow[] {
  const leftFlat = flatten(left);
  const rightFlat = flatten(right);
  const paths = Array.from(new Set([...Object.keys(leftFlat), ...Object.keys(rightFlat)])).sort();

  return paths.map((path) => {
    const l = leftFlat[path];
    const r = rightFlat[path];
    // null (explicit in C#'s JSON) and undefined (an absent key in the TS object) mean the same
    // "not set" thing here, not a real mismatch -- normalize both to null before comparing.
    const mismatch = (l ?? null) !== (r ?? null);
    return { path, left: l, right: r, mismatch };
  });
}

export function describeSection(path: string): string {
  if (path.startsWith('result.udiDi.')) {
    return 'UDI-DI — IFA_UDI_Parser_Analysis.md §2';
  }
  if (path.startsWith('result.udiPi.')) {
    return 'UDI-PI — IFA_UDI_Parser_Analysis.md §3';
  }
  if (path.startsWith('error.')) {
    return 'Build/parse error';
  }
  return '';
}
