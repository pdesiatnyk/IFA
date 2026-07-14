export interface SampleUdi {
  category: string;
  label: string;
  barcode: string;
}

/**
 * Hand-picked from shared-fixtures/udi-test-cases.json's valid barcodeFixtures entries — kept in
 * sync manually with that file, the same pattern the parser test suites already follow for
 * their own fixtures.
 */
export const SAMPLE_UDIS: SampleUdi[] = [
  { category: 'PPN', label: 'UDI-DI only', barcode: '(9N)111234567842' },
  {
    category: 'PPN',
    label: 'Full: lot + expiry + serial',
    barcode: '(9N)111234567842(1T)1A234B5(D)151231(S)1234567890123456',
  },
  {
    category: 'PPN',
    label: 'Non-canonical field order',
    barcode: '(9N)111234567842(S)JXCC263D0889(1T)170400XYZ(D)230617',
  },
  { category: 'HPC', label: 'Unit of use (packaging level 0)', barcode: '(9N)1312345MED777094' },
  { category: 'HPC', label: 'Pack of 3 (packaging level 2)', barcode: '(9N)1312345MED777227' },
  { category: 'Master UDI-DI', label: 'Device group MAX19', barcode: '(9N)MA12345MAX1900' },
  {
    category: 'Envelope forms',
    label: 'DIN 16598 keyboard form (HPC only)',
    barcode: '.9N1312345MED777094^1TABC12345^D241231',
  },
];
