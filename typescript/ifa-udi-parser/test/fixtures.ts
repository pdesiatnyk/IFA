import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import path from 'node:path';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const FIXTURES_PATH = path.resolve(__dirname, '../../../shared-fixtures/udi-test-cases.json');

export interface CheckDigitFixture {
  input: string;
  expected: string;
  note?: string;
}

export interface ExpectedUdiDi {
  raw: string;
  scheme: string;
  praCode: string;
  pzn?: string;
  cin?: string;
  itemReference?: string;
  packagingLevelIndex?: number;
  deviceGroupCode?: string;
  checkDigits: string;
}

export interface ExpectedUdiPi {
  lot?: string;
  expiryDate?: string;
  manufacturingDate?: string;
  serialNumber?: string;
  quantity?: number;
  price?: string;
  url?: string;
  additionalGtins?: string[];
}

export interface BarcodeFixture {
  name: string;
  input: string;
  inputForm: string;
  expectedValid: boolean;
  reason?: string;
  note?: string;
  expected?: { udiDi: ExpectedUdiDi; udiPi: ExpectedUdiPi };
}

export interface FixtureFile {
  checkDigitFixtures: {
    mod97: CheckDigitFixture[];
    mod11Pzn: CheckDigitFixture[];
  };
  barcodeFixtures: BarcodeFixture[];
}

export function loadFixtures(): FixtureFile {
  const json = readFileSync(FIXTURES_PATH, 'utf-8');
  return JSON.parse(json) as FixtureFile;
}
