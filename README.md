# IFA UDI Barcode Parsers

Parser and validator libraries for IFA UDI barcodes used on medical devices under EU MDR/IVDR
(PPN, HPC, and Master UDI-DI schemes). Implemented twice, once in C# and once in TypeScript,
with matching APIs and a shared set of test fixtures so both stay in sync.

Each library exposes two functions:

- **`check(barcode)`** — returns `true` if the input is a structurally and arithmetically valid
  IFA UDI barcode (mod-97 / mod-11 check digits included), `false` otherwise.
- **`parseUdi(barcode)`** — decomposes a valid barcode into its `UDI-DI` and `UDI-PI` parts.
  Throws a domain-specific error for invalid input.

## Repository layout

```
documentation/                    IFA source specs (PDF -> markdown) + distilled analysis doc
  IFA_UDI_Parser_Analysis.md      Start here for the technical spec behind these parsers
csharp/                           C# implementation (IfaUdi.Parser)
typescript/ifa-udi-parser/        TypeScript implementation (ifa-udi-parser)
shared-fixtures/
  udi-test-cases.json             Fixtures consumed by both test suites, keeps them in parity
```

`documentation/IFA_UDI_Parser_Analysis.md` is the source of truth for the encoding rules
(UDI-DI/UDI-PI structure, check-digit algorithms, Data Identifier table, envelope syntax, and
what's explicitly out of scope) — read it before changing either parser.

## What's in scope

- UDI-DI carriers: **PPN** (PZN-based, PRA-Code `11`), **HPC** (PRA-Code `13`), **Master UDI-DI**
  (PRA-Code `MA`). Basic UDI-DI is not barcode-parsed (it never appears on a barcode).
- UDI-PI elements: lot/batch (`1T`), expiry date (`D`), manufacturing date (`16D`), serial number
  (`S`), quantity (`Q`), price (`27Q`), URL (`33L`), additional NTIN/GTIN (`8P`).
- Input auto-detected across three envelope forms: raw ISO/IEC 15434 Format 06 (with control
  characters), the DIN 16598 keyboard-compatible form (HPC only), and the printable
  "Interpretation Line" form, e.g. `(9N)111234567842(1T)ABC12345(D)241231`.

Explicitly out of scope: legacy Code 39 PZN-only barcodes, GS1 Application Identifier encoding,
and IFA's transport/logistics License Plate scheme — see the analysis doc for the rationale.

## C# — `csharp/IfaUdi.Parser`

```bash
cd csharp
dotnet test
```

```csharp
using IfaUdi.Parser;

bool valid = UdiParser.Check("(9N)111234567842(1T)ABC12345(D)241231");

var parsed = UdiParser.ParseUdi("(9N)111234567842(1T)ABC12345(D)241231");
// parsed.UdiDi.Scheme == UdiScheme.Ppn, parsed.UdiDi.Pzn == "12345678"
// parsed.UdiPi.Lot == "ABC12345", parsed.UdiPi.ExpiryDate == "2024-12-31"
```

`ParseUdi` throws `IfaUdiFormatException` for invalid input — call `Check` first if you want a
non-throwing path.

## TypeScript — `typescript/ifa-udi-parser`

```bash
cd typescript/ifa-udi-parser
npm install
npm test
npm run build
```

```ts
import { check, parseUdi } from 'ifa-udi-parser';

const valid = check('(9N)111234567842(1T)ABC12345(D)241231');

const parsed = parseUdi('(9N)111234567842(1T)ABC12345(D)241231');
// parsed.udiDi.scheme === 'PPN', parsed.udiDi.pzn === '12345678'
// parsed.udiPi.lot === 'ABC12345', parsed.udiPi.expiryDate === '2024-12-31'
```

`parseUdi` throws `IfaUdiFormatError` for invalid input — call `check` first if you want a
non-throwing path.

## Tests and fixtures

Both test suites read `shared-fixtures/udi-test-cases.json` directly, so a new fixture added
there is automatically exercised by both languages. It contains:

- `checkDigitFixtures` — direct mod-97 / mod-11 arithmetic cases.
- `barcodeFixtures` — full `check()`/`parseUdi()` cases, valid and invalid, including
  non-canonical field ordering and each out-of-scope category.

Run everything:

```bash
(cd csharp && dotnet test)
(cd typescript/ifa-udi-parser && npm test)
```
