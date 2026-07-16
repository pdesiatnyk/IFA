# Example Serials

Synthetic, checksum-valid example serials generated for this project. These are **not** real registered PZNs or AIM codes — sequential test bases (`1000001+`, `2000001+`) were used, following the same pattern as `shared-fixtures/udi-test-cases.json`. Each value is checksum-correct per `csharp/IfaUdi.Parser/CheckDigits.cs` (Mod-11 for the PZN base, Mod-97 outer check for the full `9N` value) and should parse successfully with `UdiParser`.

## PZN serials (PPN, PRA-Code `11`, format `(9N)11{PZN8}{CC2}`)

| # | Base | PZN (base + Mod-11 check) | Full value |
|---|---|---|---|
| 1 | 1000001 | 10000018 | `(9N)111000001826` |
| 2 | 1000002 | 10000024 | `(9N)111000002489` |
| 3 | 1000003 | 10000030 | `(9N)111000003055` |
| 4 | 1000004 | 10000047 | `(9N)111000004745` |
| 5 | 1000005 | 10000053 | `(9N)111000005311` |
| 6 | 1000007 | 10000076 | `(9N)111000007664` |
| 7 | 1000008 | 10000082 | `(9N)111000008230` |
| 8 | 1000009 | 10000099 | `(9N)111000009920` |
| 9 | 1000010 | 10000107 | `(9N)111000010714` |
| 10 | 1000011 | 10000113 | `(9N)111000011377` |

Base `1000006` is skipped: its Mod-11 remainder is 10, which per spec is never issued as a real PZN.

## AIM serials (Portugal national code, PRA-Code `17`, format `(9N)17{code}{CC2}`)

| # | National code | Full value |
|---|---|---|
| 1 | 2000001 | `(9N)17200000114` |
| 2 | 2000002 | `(9N)17200000224` |
| 3 | 2000003 | `(9N)17200000334` |
| 4 | 2000004 | `(9N)17200000444` |
| 5 | 2000005 | `(9N)17200000554` |
| 6 | 2000006 | `(9N)17200000664` |
| 7 | 2000007 | `(9N)17200000774` |
| 8 | 2000008 | `(9N)17200000884` |
| 9 | 2000009 | `(9N)17200000994` |
| 10 | 2000010 | `(9N)17200001013` |
