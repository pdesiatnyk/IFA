# UDI-DI Structure and Field Reference

This document summarizes the structure of the UDI-DI (Device Identifier) as defined by the
IFA Coding System, and how it maps onto the `UdiScheme` enum and `UdiDi` class in this
repository's parser (`csharp/IfaUdi.Parser/Models/UdiDi.cs`,
`csharp/IfaUdi.Parser/Models/UdiScheme.cs`, `csharp/IfaUdi.Parser/UdiParser.cs`). It is a
companion/summary document, not a replacement for the source specifications in this folder
(see `IFA-Info_Spec_UDI_Code_EN (1).md`, `IFA_Spec_PPN_Code_Handelspackung_EN.md`,
`IFA_UDI_Parser_Analysis.md`).

New to this topic? [`UDI_Explained_Simply.md`](UDI_Explained_Simply.md) covers the same ground
in plain language with real-world analogies, no prior barcode/UDI knowledge required.

## 1. UDI-DI in the IFA / MDR-IVDR standard

Under EU MDR (2017/745) / IVDR (2017/746), a **UDI** (Unique Device Identification) consists of:

- **UDI-DI** (Device Identifier) — a static code identifying the device/trade item at a given
  packaging level. Different pack sizes require different UDI-DIs.
- **UDI-PI** (Production Identifier) — variable per-unit data: lot/batch, expiry date,
  manufacturing date, serial number.

IFA is designated as an EU **Issuing Entity** for UDI. The UDI-DI is always carried in the Data
Matrix under Data Identifier **`9N`**, and comes in one of five formats. The sub-scheme is
disambiguated by the leading characters of the `9N` payload (the PRA-Code):

| Scheme | PRA-Code | Payload structure | Total length | Character set |
|---|---|---|---|---|
| PPN (containing PZN) | `11` | `11` + 8-digit PZN + 2-digit check digits | 12 | digits `0-9` only |
| HPC (Health Product Code) | `13` | `13` + 5-digit CIN + item/part number (1–18 chars) + 1-digit packaging-level index (`0`–`8`) + 2-digit check digits | 11–28 | CIN: `0-9A-Z`; part number: `0-9A-Z.-` (no lowercase); PLI: `0-9` |
| Master UDI-DI (MUDI) | `MA` | `MA` + 5-digit CIN + device group code (1–19 chars) + 2-digit check digits | 10–28 | `0-9A-Z.-` |
| AIC (Italy) | `15` | `15` + national code (1–18 chars, opaque) + 2-digit check digits | 5–22 | national code: `0-9A-Z.-` (provisional bound — see §4) |
| AIM (Portugal) | `17` | `17` + national code (1–18 chars, opaque) + 2-digit check digits | 5–22 | national code: `0-9A-Z.-` (provisional bound — see §4) |

All five variants use the same **Modulo 97** check-digit algorithm, applied to the full string
including the PRA-Code prefix, excluding the trailing 2 check-digit characters: map each
character to its ASCII value, weight by position starting at 2 and incrementing by 1, sum the
products, and take `sum mod 97` zero-padded to 2 digits.

Related, but not represented as a barcode field:

- **Basic UDI-DI (BUDI)** — PRA-Code `PP`, never barcoded; it's an EUDAMED-only regulatory key
  (XML tag `B_UDI_DI`), used here only as a cross-check fixture for the shared Mod-97 routine.

## 2. `UdiScheme` enum

```csharp
public enum UdiScheme
{
    Ppn,
    Hpc,
    MasterUdiDi,
    Aic,
    Aim,
}
```

Discriminates which of the five barcoded UDI-DI variants a parsed `UdiDi` represents.
`UdiParser.ParseUdiDi` selects it by sniffing the leading characters of the `9N` payload:
`"11"` → `Ppn`, `"13"` → `Hpc`, `"MA"` → `MasterUdiDi`, `"15"` → `Aic`, `"17"` → `Aim`. Any
other prefix throws `IfaUdiFormatException`. PRA-Codes `12`, `14`, `16`, `18`–`24` (see §4
below) remain recognized-but-unsupported; `15` and `17` are now supported, but only with
opaque validation (see §4).

## 3. `UdiDi` class fields

```csharp
public sealed class UdiDi
{
    public string Raw { get; set; }
    public UdiScheme Scheme { get; set; }
    public string PraCode { get; set; }
    public string CheckDigits { get; set; }
    public string Pzn { get; set; }
    public string Cin { get; set; }
    public string ItemReference { get; set; }
    public int? PackagingLevelIndex { get; set; }
    public string DeviceGroupCode { get; set; }
    public string NationalCode { get; set; }
}
```

This is a union-style model: depending on `Scheme`, only a subset of fields is populated.

| Field | Meaning | Populated for | Format |
|---|---|---|---|
| `Raw` | The full original `9N` payload string as parsed | all | e.g. `"111234567842"` |
| `Scheme` | Which variant this is | all | `Ppn` / `Hpc` / `MasterUdiDi` / `Aic` / `Aim` |
| `PraCode` | The 2-char Product Registration Agency Code prefix | all | `"11"`, `"13"`, `"MA"`, `"15"`, or `"17"` |
| `CheckDigits` | Trailing 2-digit Mod-97 check digits, validated against the rest of the string | all | e.g. `"42"` |
| `Pzn` | 8-digit Pharmazentralnummer (Germany's national code), itself validated with its own Mod-11 check digit | **PPN only** | 8 digits, e.g. `"12345678"` |
| `Cin` | 5-digit/char IFA-assigned Company/Manufacturer Identification Number (supplier number) | **HPC and Master UDI-DI** | `0-9A-Z`, 5 chars |
| `ItemReference` | Manufacturer's own item/part number | **HPC only** | 1–18 chars, `0-9A-Z.-`, no lowercase |
| `PackagingLevelIndex` | Packaging level, manufacturer-defined meaning (e.g. 0=unit of use, 1=single pack, 2=pack of 5, ...); `9` is reserved/invalid and rejected | **HPC only** | int 0–8 |
| `DeviceGroupCode` | Manufacturer's designation of the product group covered by this Master UDI-DI | **Master UDI-DI only** | 1–19 chars, `0-9A-Z.-` |
| `NationalCode` | Opaque Italy AIC / Portugal AIM code; IFA does not document an inner format, so only overall bounds are enforced (no inner check-digit, unlike `Pzn`) | **AIC and AIM only** | 1–18 chars, `0-9A-Z.-` (provisional) |

Per scheme, from `UdiParser.cs`:

- **PPN** populates `Raw`, `Scheme=Ppn`, `PraCode="11"`, `Pzn`, `CheckDigits` — `Cin`,
  `ItemReference`, `PackagingLevelIndex`, `DeviceGroupCode`, `NationalCode` stay `null`.
- **HPC** populates `Raw`, `Scheme=Hpc`, `PraCode="13"`, `Cin`, `ItemReference`,
  `PackagingLevelIndex`, `CheckDigits` — `Pzn`, `DeviceGroupCode`, `NationalCode` stay `null`.
- **Master UDI-DI** populates `Raw`, `Scheme=MasterUdiDi`, `PraCode="MA"`, `Cin`,
  `DeviceGroupCode`, `CheckDigits` — `Pzn`, `ItemReference`, `PackagingLevelIndex`,
  `NationalCode` stay `null`.
- **AIC** populates `Raw`, `Scheme=Aic`, `PraCode="15"`, `NationalCode`, `CheckDigits` — all
  other scheme-specific fields stay `null`.
- **AIM** populates `Raw`, `Scheme=Aim`, `PraCode="17"`, `NationalCode`, `CheckDigits` — all
  other scheme-specific fields stay `null`.

## 4. Where a "National Code" lives

The PPN structure generalizes beyond the German PZN: the PPN spec's PRA-Code table
(`IFA_Spec_PPN_Code_Handelspackung_EN.md`) registers the same structural slot — **the payload
segment between the 2-digit PRA-Code prefix and the 2-digit check digits** — for other
countries' national pharmacy-product codes:

| PRA-Code | National code |
|---|---|
| `11` | PZN (Germany) |
| `12` | Registered Blood Product Number (EUROCODE IBLS) |
| `14` | CNK code (Belgium) |
| `15` | AIC code (Italy) |
| `16` | PZN-Austria |
| `17` | Registration Number of Medicine Presentation (Portugal) |
| `18` | Z-Index (Netherlands) |
| `19` | NENSI code (Slovenia) |
| `20`–`24` | CIP/ACL/UCD codes (France) |

In `UdiDi`, PRA-Codes `11` (PZN), `15` (AIC), and `17` (AIM) are implemented:

- `11` is surfaced as `Pzn`, with **full inner validation** — an 8-digit format and its own
  Mod-11 check digit, exactly as documented by IFA.
- `15` and `17` are surfaced as the shared `NationalCode` field with **opaque validation
  only**. IFA's PRA-Code registry does not publish a length, charset, or inner check-digit
  specification for these two codes (confirmed absent from
  `IFA_Spec_PPN_Code_Handelspackung_EN.md`'s PRA-Code table, and not found via external
  search for Italian AIC / INFARMED AIM documentation). This parser therefore enforces only a
  **provisional** bound — 1–18 chars, `0-9A-Z.-` (the same bound already used for HPC's
  `ItemReference`) — plus the outer Mod-97 checksum. Revisit and tighten this bound if an
  authoritative inner-format spec is ever found.

PRA-Codes `12`, `14`, `16`, `18`–`24` remain unimplemented: `UdiParser.ParseUdiDi` rejects
them rather than mapping them to a field, since there is no equivalent property (e.g. no `Cnk`)
for those national codes today.

## 5. Related model: `UdiPi`

For completeness, the Production Identifier (`UdiPi`) carries the variable per-unit data that
accompanies a UDI-DI in the same Data Matrix payload — lot/batch (`1T`), expiry date (`D`),
manufacturing date (`16D`), serial number (`S`), quantity (`Q`), price (`27Q`), URL (`33L`),
and additional GTIN/NTIN (`8P`). See `csharp/IfaUdi.Parser/Models/UdiPi.cs` and
`UdiParser.ParseUdiPi` for details; it is out of scope for this document, which focuses on
UDI-DI only.
