# IFA UDI Barcode Parsers

Parser, builder, and validator libraries for IFA UDI barcodes used on medical devices under EU
MDR/IVDR (PPN, HPC, and Master UDI-DI schemes). Implemented twice, once in C# and once in
TypeScript, with matching APIs and a shared set of test fixtures so both stay in sync. A Vite +
Vue showcase app cross-compares the two implementations side by side.

Each library exposes three functions:

- **`check(barcode)`** — returns `true` if the input is a structurally and arithmetically valid
  IFA UDI barcode (mod-97 / mod-11 check digits included), `false` otherwise.
- **`parseUdi(barcode)`** — decomposes a valid barcode into its `UDI-DI` and `UDI-PI` parts.
  Throws a domain-specific error for invalid input.
- **`buildUdi(input, envelopeForm?)`** — the inverse of `parseUdi`: constructs a valid barcode
  string from structured UDI-DI/UDI-PI input. Check digits are always computed, never
  user-supplied. `envelopeForm` selects the output form (Interpretation Line by default, raw
  ISO/IEC 15434, or DIN 16598 for HPC). Throws a domain-specific build error on invalid input.

## Repository layout

```
documentation/                    IFA source specs (PDF -> markdown) + distilled analysis doc
  IFA_UDI_Parser_Analysis.md      Start here for the technical spec behind these parsers
csharp/
  IfaUdi.Parser/                  C# parser + builder library
  IfaUdi.Parser.Tests/            xUnit tests
  IfaUdi.Parser.Api/              Thin ASP.NET Core minimal API wrapping the library, used by the showcase
typescript/ifa-udi-parser/        TypeScript parser + builder library (npm package "ifa-udi-parser")
showcase/                         Vite + Vue 3 SPA cross-comparing the TS and C# implementations
shared-fixtures/
  udi-test-cases.json             Fixtures consumed by both test suites, keeps them in parity
package.json                      npm workspace root (typescript/ifa-udi-parser + showcase)
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

```csharp
using IfaUdi.Parser.Models;

var input = new BuildUdiInput
{
    UdiDi = new BuildUdiDiInput { Scheme = UdiScheme.Ppn, PznBase = "1234567" }, // check digits auto-computed
    UdiPi = new BuildUdiPiInput { Lot = "ABC12345", ExpiryDate = "2024-12-31" },
};
string barcode = UdiBuilder.BuildUdi(input); // "(9N)111234567842(1T)ABC12345(D)241231"
```

`BuildUdi` throws `IfaUdiBuildException` (with `Field` and `Reason`) for invalid input.

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

```ts
import { buildUdi } from 'ifa-udi-parser';

const barcode = buildUdi({
  udiDi: { scheme: 'PPN', pznBase: '1234567' }, // check digits auto-computed
  udiPi: { lot: 'ABC12345', expiryDate: '2024-12-31' },
}); // "(9N)111234567842(1T)ABC12345(D)241231"
```

`buildUdi` throws `IfaUdiBuildError` (with `field` and `reason`) for invalid input. Pass an
`envelopeForm` as the second argument (`'interpretationLine'` (default) | `'rawIso15434'` |
`'din16598'`, the last only valid for the HPC scheme) to select the output form.

## Showcase — `showcase/` + `csharp/IfaUdi.Parser.Api`

A Vite + Vue 3 SPA with two tabs: **Compare** (parses a barcode with both the in-browser
TypeScript library and the C# library over HTTP, showing pass/fail cards and a field-by-field
diff) and **Generate** (the builder form — pick a scheme, fill in UDI-DI/UDI-PI fields and an
envelope form, build with both languages, and optionally send the result into the Compare tab).

Requires the C# API running first (it serves `/api/check`, `/api/parse`, `/api/build` on
`:5081`; the Vite dev server proxies `/api/*` to it):

```bash
# terminal 1 — API
dotnet run --project csharp/IfaUdi.Parser.Api

# terminal 2 — workspace install (root) + showcase dev server
npm install
npm run dev --workspace=showcase
```

Then open the printed `http://localhost:5173` (or whatever port Vite falls back to if that one's
taken) URL. If you change `typescript/ifa-udi-parser`'s source, rebuild it
(`npm run build --workspace=typescript/ifa-udi-parser`) so the showcase picks up the change
through the npm workspace link.

## Tests and fixtures

Both test suites read `shared-fixtures/udi-test-cases.json` directly, so a new fixture added
there is automatically exercised by both languages. It contains:

- `checkDigitFixtures` — direct mod-97 / mod-11 arithmetic cases.
- `barcodeFixtures` — full `check()`/`parseUdi()` cases, valid and invalid, including
  non-canonical field ordering and each out-of-scope category.
- `buildFixtures` — full `buildUdi()`/`BuildUdi()` cases (one per scheme, one per envelope form,
  plus invalid-input cases), each valid case also round-tripped through `parseUdi()`/`ParseUdi()`
  to prove the builder is a true inverse of the parser.

Run everything:

```bash
(cd csharp && dotnet test)
(cd typescript/ifa-udi-parser && npm test)
```
