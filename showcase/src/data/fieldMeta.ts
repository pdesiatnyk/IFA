/**
 * Human-readable descriptions for parsed/build fields, each citing the section of
 * documentation/IFA_UDI_Parser_Analysis.md it's drawn from.
 */
export const FIELD_META: Record<string, string> = {
  'result.udiDi.raw': 'Full UDI-DI payload string, Data Identifier "9N" (§2)',
  'result.udiDi.scheme': 'Which of the three UDI-DI carriers: PPN, HPC, or MASTER_UDI_DI (§2)',
  'result.udiDi.praCode': 'Product Registration Agency Code prefix: 11=PPN, 13=HPC, MA=Master UDI-DI (§2)',
  'result.udiDi.checkDigits': 'Modulo-97 check digits, computed over prefix+payload (§5.2)',
  'result.udiDi.pzn': 'Pharmazentralnummer, PPN scheme only. Last digit is a modulo-11 check digit (§5.1)',
  'result.udiDi.cin': 'Company Identification Number, IFA-assigned 5-char code (HPC / Master UDI-DI) (§2)',
  'result.udiDi.itemReference': "Manufacturer's item/part reference, HPC only (§2)",
  'result.udiDi.packagingLevelIndex': '0-8; manufacturer-defined packaging level, HPC only (9 is reserved/invalid) (§2)',
  'result.udiDi.deviceGroupCode': 'Manufacturer-defined product group designation, Master UDI-DI only (§2)',
  'result.udiPi.lot': 'Lot/batch number, Data Identifier "1T" (§3)',
  'result.udiPi.expiryDate': 'Expiry date, Data Identifier "D" (encoded YYMMDD in the barcode) (§3)',
  'result.udiPi.manufacturingDate': 'Manufacturing date, Data Identifier "16D" (encoded YYYYMMDD) (§3)',
  'result.udiPi.serialNumber': 'Serial number, Data Identifier "S" (§3)',
  'result.udiPi.quantity': 'Quantity, Data Identifier "Q" (§3)',
  'result.udiPi.price': 'Price, Data Identifier "27Q", format 0.00 (§3)',
  'result.udiPi.url': 'Hyperlink, Data Identifier "33L" (§3)',
  'result.udiPi.additionalGtins': 'Additional NTIN/GTIN(s), Data Identifier "8P", 14 digits each (§3)',
  'error.message': 'Human-readable error message',
  'error.field': 'Input field path the error applies to',
  'error.reason': 'Machine-readable error reason code',
};
