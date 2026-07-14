# IFA UDI Barcode — Distilled Technical Analysis for Parser Implementation

This document distills the six IFA source specifications in this folder into the subset of
structural rules, algorithms, and worked examples needed to implement `check()` and
`parseUdi()` parsers for IFA UDI barcodes (C# and TypeScript). It is derived from, and should
be read as a companion to, the source PDFs — not a replacement for them.

## 1. Scope

### In scope

| Source document | Used for |
|---|---|
| `IFA-Info_Spec_UDI_Code_EN (1).md` | UDI-DI/UDI-PI structure, Data Identifier table (Appendix A), ISO/IEC 15434 data string layout, worked examples |
| `IFA_Spec_PPN_Code_Handelspackung_EN.md` | PZN/PPN digit structure, PRA-Code table, Data/Application Identifier reference table, additional worked examples |
| `IFA-Info_Check_Digit_Calculations_PZN_PPN_UDI_EN (1).md` | PZN (mod 11) and PPN/HPC/Master-UDI-DI/Basic-UDI-DI (mod 97) check-digit algorithms, with fully worked arithmetic |

### Explicitly out of scope (and why)

| Source document | Reason excluded |
|---|---|
| `IFA_Info_Code_39_EN.md` | Describes a legacy, PZN-only linear barcode (`*-12345678*`). It cannot carry UDI-PI (lot/expiry/serial) at all, and since 9 Feb 2019 it is optional wherever a Data Matrix UDI code is present. Not a UDI barcode. |
| `IFA_Form_Request_PRA_Code_EN.md` | A pure administrative order form (+ terms & conditions) for requesting a new PRA-Code from IFA. No technical/parsing content, no PRA-Code reference table. |
| `IFA_Spec_Transport_Logistik_EN.md` | Covers shipping/logistics unit identification ("License Plate" scheme, Data Identifiers `J`/`L`/`2L`/`25L` under ISO/IEC 15459), a structurally unrelated barcode family. Shipping containers are explicitly exempt from UDI-DI requirements per the UDI spec itself. |

Additionally, by design decision (not a documentation gap):
- **GS1 Application Identifier (AI) encoding** (e.g. AI `01`/`10`/`17`/`21`) is a parallel encoding scheme documented alongside IFA's own ASC/Data-Identifier scheme, but it is GS1's standard, not IFA's. Only the **ASC / Data Identifier format** (`9N`, `1T`, `D`, `16D`, `S`, ...) is targeted.
- **Basic UDI-DI** (PRA-Code `PP`) is never encoded in a barcode — it has no Data Identifier and exists only as an XML tag (`B_UDI_DI`) for EUDAMED registration. It is not a `parseUdi()` input, though its check-digit algorithm (identical mod-97 routine) is used as a cross-check fixture.
- **Code 39 PZN barcodes** are out of scope per above — `check()` returns `false` for a bare Code-39-style PZN string since it is not a UDI barcode.

## 2. UDI-DI structure

Every UDI-DI (and Master UDI-DI) is carried under Data Identifier **`9N`**. The sub-scheme is
disambiguated by the payload's leading characters (the PRA-Code):

| Scheme | PRA-Code | Payload structure | Total length | Character set |
|---|---|---|---|---|
| PPN (containing PZN) | `11` | `11` + 8-digit PZN + 2-digit check digits | 12 | digits `0-9` only |
| HPC (Health Product Code) | `13` | `13` + 5-digit CIN + supplier part number (1–18 chars; a 4-digit-numeric-only variant also exists) + 1-digit packaging-level index (`0`–`8`) + 2-digit check digits | 11–28 | CIN: `0-9A-Z`; part number: `0-9A-Z.-` (no lowercase); PLI: `0-9` |
| Master UDI-DI | `MA` | `MA` + 5-digit CIN + device group code (1–19 chars) + 2-digit check digits | 10–28 | `0-9A-Z.-` |

Notes:
- The **packaging-level index** (HPC only) is a single digit placed immediately before the
  check digits. `0`–`8` are valid (manufacturer-defined meaning, e.g. `0`=unit of use,
  `1`=single pack, `2`=pack of 5, ...). `9` is reserved for variable quantities and is
  **invalid** for UDI — `check()` must reject it.
- Field ordering of `9N` vs `1T`/`D`/`S`/etc. within the overall barcode is not fixed (see §4).
- Other PRA-Codes exist in the IFA registry (e.g. `12`, `14`–`24` for other national
  pharmacy-product schemes, `00–09`/`25–99`/most of `AA–ZZ` reserved/unassigned) but are
  **not implemented** — a `9N` payload with a recognized-but-unsupported PRA-Code should be
  treated as a parse failure (`check()` → `false`), not silently mis-parsed as PPN/HPC/MA.

### Basic UDI-DI (reference only, not a barcode input)

`PP` + 5-digit CIN + device group code (1–16 chars, `0-9A-Z.`) + 2-digit check digits.
Example: `PP12345ABCD.12345678.9004`. Used only to validate the shared mod-97 routine.

## 3. UDI-PI structure

UDI-PI elements use ANSI MH10.8.2 Data Identifiers:

| DI | Element | Format | Length | Character set |
|---|---|---|---|---|
| `1T` | Lot / batch number (LOT) | free text | 1–20 | Alphanumeric. Excludes ASCII 35 `#`, 36 `$`, 64 `@`, 91 `[`, 92 `\`, 93 `]`, 94 `^`, 96 `` ` ``, 123 `{`, 124 `|`, 125 `}`, 126 `~`, 127, all control chars (0–31), and all non-ASCII (>127). |
| `D` | Expiry date (EXP) | `YYMMDD` | 6 | digits; `YY` → 2000–2099; `MM` = `01`–`12`; `DD` = `00` (unspecified day) or `01`–`31` |
| `16D` | Manufacturing date (MFD) | `YYYYMMDD` | 8 | digits |
| `S` | Serial number (SN) | free text | 1–20 | same rule as `1T` |
| `Q` | Quantity | numeric | 1–8 | digits |
| `27Q` | Price | `0.00` | 4–20 | digits + `.` |
| `33L` | URL | free text | unspecified | — |
| `8P` | Additional NTIN/GTIN | numeric | 14 | digits |

`D` (bare) always means expiry date in this context (ANSI MH10.8.2 defines `D` as a generic
date; the IFA/UDI usage fixes it to expiry). Do not confuse it with `16D` (manufacturing date).

## 4. Data string envelope (the "raw" barcode payload)

The UDI spec references ISO/IEC 15434 Format 06 for the overall envelope, but only gives
placeholder tokens (`*Mac06*`, `*Gs*`) rather than literal byte values. The literal values below
are the standard ISO/IEC 15434 control characters (external to the IFA docs, required to parse
raw scanner output):

```
[)>  <RS>  06  <GS>  <DI><value> <GS> <DI><value> <GS> ... <DI><value>  <RS>  <EOT>
```

- `<RS>` = 0x1E (Record Separator), `<GS>` = 0x1D (Group Separator), `<EOT>` = 0x04 (End of
  Transmission). `[)>` are three literal characters.
- On a Data Matrix symbol this envelope is typically carried via ISO/IEC 16022 "Macro 06" mode;
  the parser operates on the already-decoded text string (post barcode-scan), not on image data,
  so macro expansion is assumed to have already happened upstream.
- Fields are **not** separated by spaces, and their order is **discretionary** — a parser must
  key off the Data Identifier tag, never position.

### Alternate envelope: DIN 16598 keyboard-compatible syntax (HPC only)

```
.9N1312345MED777094^1TABC12345^D241231
```
Leading `.` in place of the `[)>` + format-06 header, `^` as the field separator, no explicit
terminator sequence.

### Normalized form: "Interpretation Line"

The spec's human-readable rendering strips all envelope control characters and brackets each
Data Identifier:
```
(9N)111234567842(1T)ABC12345(D)241231
```
This is also accepted directly as parser input (per your input-format decision), and is the
internal normalization target both other envelope forms are converted to before field
extraction: detect envelope type by leading bytes/characters, extract `(DI, value)` pairs, then
process identically regardless of original form.

## 5. Check digit algorithms

### 5.1 PZN — modulo 11

Applies to the 7-digit base of a PZN. Each digit weighted `1..7` left to right; sum the
products; check digit = `sum mod 11` (single digit, 0–9). A remainder of 10 means the digit
sequence is invalid and is never issued as a PZN.

**Worked example** (from source doc): PZN base `2758089` →
weights `1,2,3,4,5,6,7` → products `2,14,15,32,0,48,63` → sum `174` → `174 mod 11 = 9` (remainder) →
check digit `9` → full PZN `27580899`.

### 5.2 PPN / HPC / Master UDI-DI / Basic UDI-DI — modulo 97

One algorithm, applied identically to all four schemes (they share the "prefix + payload +
2-digit check" shape):

1. Take the full string **including** the PRA-Code/IAC prefix, **excluding** the trailing 2
   check-digit characters.
2. Map each character to its ASCII decimal value (digits `0`–`9` → 48–57, `A`–`Z` → 65–90,
   `.` → 46, `-` → 45).
3. Weight each character's ASCII value by a factor starting at **2** for the leftmost character
   and incrementing by 1 for each subsequent character.
4. Sum all (ASCII value × weight) products.
5. `check digits = sum mod 97`, taken as a **numeric value** (not re-mapped through ASCII) and
   zero-padded to 2 digits.

**Worked example 1** (PPN): input `1103752864` (PRA `11` + PZN `03752864`) → ASCII values
`49,49,48,51,55,53,50,56,54,52` → weights `2..11` → products
`98,147,192,255,330,371,400,504,540,572` → sum `3409` → `3409 mod 97 = 14` → check digits `14`
→ full PPN `110375286414` (matches the source doc's stated result exactly; use as a test
fixture). Note this is a *different* worked PZN (`03752864`) than the `111234567842` example
used elsewhere in this document (PZN `12345678`, check `42`) — both are valid, independent
fixtures and must not be conflated.

**Worked example 2** (Basic UDI-DI, validates the shared routine): input
`PP12345ABCD.12345678.90` (23 chars) → weights `2..24` → sum `16203` →
`16203 mod 97 = 4` → check digits `04` → full value `PP12345ABCD.12345678.9004`.

This routine is implemented **once**, parameterized by the input substring, and reused for PPN,
HPC, and Master UDI-DI validation inside `check()`/`parseUdi()`.

## 6. Cross-language test fixtures

These worked examples (verified arithmetically above, or taken verbatim from the source specs)
should back a shared fixture set consumed by both the C# and TypeScript test suites:

| Case | Raw value | Notes |
|---|---|---|
| PPN UDI-DI | `111234567842` | PRA `11` + PZN `12345678` + check `42` |
| HPC UDI-DI, unit of use | `1312345MED777094` | PRA `13`, CIN `12345`, item ref `MED777`, PLI `0`, check `94` |
| HPC UDI-DI, pack of 3 | `1312345MED777227` | same item, PLI `2`, check `27` — validates PLI affects check digit |
| Master UDI-DI | `MA12345MAX1900` | PRA `MA`, CIN `12345`, device group `MAX19`, check `00` |
| Basic UDI-DI (non-barcode) | `PP12345ABCD.12345678.9004` | mod-97 cross-check only |
| Full Interpretation Line | `(9N)111234567842(1T)1A234B5(D)151231(S)1234567890123456` | batch + expiry + serial |
| Non-canonical field order | `(9N)111234567842(S)JXCC263D0889(1T)170400XYZ(D)230617` | SN before LOT before EXP — parser must not assume order |
| PZN mod-11 | base `2758089` → full `27580899` | check digit `9` |
| Invalid: bad check digit | `111234567843` | same as valid PPN fixture with last digit corrupted |
| Invalid: PLI = 9 | `1312345MED777099` (illustrative) | packaging-level index 9 is reserved/invalid for UDI |
| Invalid: Code 39 PZN string | `*-12345678*` | out of scope — `check()` must return `false` |

## 7. Open items / assumptions carried into implementation

- Barcode scanner/decoder integration (image → text) is entirely out of scope; parsers accept
  only the already-decoded text string.
- A `9N` payload with a PRA-Code other than `11`/`13`/`MA` is a recognized-but-unsupported
  scheme and should fail `check()` rather than being force-parsed.
- `parseUdi()` throws a domain-specific format error on invalid input rather than returning
  null, so callers wanting a non-throwing path call `check()` first.
