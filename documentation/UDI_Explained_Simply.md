# UDI Explained Simply

A plain-language companion to [`UDI_DI_Structure_And_Fields.md`](UDI_DI_Structure_And_Fields.md). That
document is the precise technical reference; this one is for anyone who just opened this repo and has
never heard of a "PRA-Code" before. No formulas here — just what each piece is *for*, with a
real-world comparison, plus one verified real example for each.

## 1. The big picture: an ID card, plus a stamp added at packing time

Every medical device or medicine pack carries two very different kinds of information, and this
project's whole job is to read both of them out of one barcode:

- **UDI-DI** ("Device Identifier") — like the **ID card printed on the product's box design**.
  It's fixed the moment the box design and pack size are decided, and every box of that exact
  product/size shares the same one. Change the pack size (single unit vs. a 3-pack) and you get a
  *new* UDI-DI, same as a new size of the same shirt gets its own barcode.
- **UDI-PI** ("Production Identifier") — like **the date and batch code stamped onto one specific
  carton** at the factory. Think of a milk carton: the carton's design and size (2%, 1 gallon) never
  changes — that's the UDI-DI part. But the "best by" date and the batch/lot code get freshly stamped
  onto each individual carton as it comes off the line — that's the UDI-PI part.

So: **UDI-DI answers "what product/pack-size is this?"**, and **UDI-PI answers "which specific batch
and unit of that product is this?"**. Section 4 below covers the UDI-PI fields; sections 2-3 cover the
UDI-DI side.

## 2. Why are there five different "ID card" formats?

IFA (the organization behind this coding system) acts a bit like a passport office. Instead of one
single design, it issues a few different "passport formats" depending on the situation — one for
German medicines, one for general health products, one for a whole product family rather than a
single pack, and so on. Every format shares one universal safety feature though: **a built-in
typo-catcher**, the same idea as the extra digit on a credit card number. If you mistype or
mis-scan even one character, the checksum won't match and the scanner immediately knows something's
wrong — before it ever looks up the wrong product. (The exact math behind that check digit is in
`UDI_DI_Structure_And_Fields.md` §1 if you're curious; you don't need it to understand what these
codes mean.)

All five formats live in the same barcode slot (technically called Data Identifier `9N`), and you can
tell them apart by their first couple of characters, the way you can tell a UK passport from a US one
by its cover.

## 3. The five UDI-DI "ID card" formats

### PPN (containing the PZN) — Germany

**Real-world comparison:** Germany already had its own well-known medicine number, the **PZN**
(Pharmazentralnummer) — every German pharmacist recognizes it, the way every US pharmacist
recognizes an NDC number. The PPN doesn't replace it; it just slides that familiar number into an
international "passport holder" so global barcode scanners (built for the IFA standard) can read it
too, with a checksum wrapped around it for safety.

**Structure:** `11` (says "this is a German PZN") + 8-digit PZN + 2-digit safety check.

**Verified example:** `(9N)111234567842` → PZN `12345678`.

### HPC (Health Product Code)

**Real-world comparison:** This is like a store's own product barcode: a code for *which
manufacturer* made it (like a store chain's supplier number), a code for *which item* (like a SKU),
and a flag for *what size pack* this is (single unit? a 3-pack? a case of 10?).

**Structure:** `13` (says "this is an HPC") + 5-character manufacturer code + item/part number (1-18
characters) + 1-digit pack-size flag (0-8) + 2-digit safety check.

**Verified examples:** `(9N)1312345MED777094` is a single unit (pack-size flag `0`) of manufacturer
`12345`'s item `MED777`; `(9N)1312345MED777227` is the exact same item packaged as a 3-pack
(pack-size flag `2`).

### Master UDI-DI (MUDI)

**Real-world comparison:** This one doesn't identify a specific box at all — it identifies a whole
**product line**, the way "iPhone 15" names a model family rather than one particular boxed phone
with its own serial number. It groups together every pack size and variant that belongs to the same
underlying product family.

**Structure:** `MA` (says "this is a Master UDI-DI") + 5-character manufacturer code + product-family
code (1-19 characters) + 2-digit safety check.

**Verified example:** `(9N)MA12345MAX1900` → manufacturer `12345`, product family `MAX19`.

### AIC (Italy) and AIM (Portugal)

**Real-world comparison:** These are like a **local courier tracking sticker**: the international
system checks that the sticker is the right shape and hasn't been smudged (well-formed length, valid
safety check), without needing to know what's actually inside the package. Only Italy's own
authority (for AIC) or Portugal's own authority (for AIM) actually knows what the code inside means —
to everyone else, it's an opaque national code.

**Structure:** `15` for AIC or `17` for AIM + national code (1-18 characters) + 2-digit safety check.

**Verified examples:** `(9N)1503222012818` (AIC), `(9N)17583412704` (AIM).

## 4. The UDI-PI fields: what gets "stamped on" per batch

Going back to the milk-carton idea — these are the fields that get freshly stamped onto one specific
physical unit or batch, not baked into the box design:

| Field | Plain-language meaning | Milk-carton analogy |
|---|---|---|
| LOT (`1T`) | The batch/production run this exact unit came from | The batch code stamped on the carton |
| Expiry date (`D`) | When this specific batch expires | The "best by" date |
| Manufacturing date (`16D`) | When this specific batch was made | The "produced on" date |
| Serial number (`S`) | This one individual unit's unique number (not shared with any other unit) | An engraved serial number on one specific carton, if it had one |
| Quantity (`Q`) | How many items are in this particular package | "Contains 12 cartons" printed on a case |
| Price (`27Q`) | A price encoded directly in the barcode | A price sticker slapped on at the store |
| URL (`33L`) | A web link printed on the pack for more info | The website printed on the side of the carton |
| Additional GTIN/NTIN (`8P`) | Extra product codes for other systems that need a different ID format for the same item | A second barcode printed on the same box for a different scanner system |

## 5. Cheat-sheet

| Prefix | Name | In plain words | Verified example |
|---|---|---|---|
| `11` | PPN (PZN) | Germany's medicine number, wrapped for global scanners | `(9N)111234567842` |
| `13` | HPC | Manufacturer + item + pack size, like a store's own barcode | `(9N)1312345MED777094` |
| `MA` | Master UDI-DI | A whole product line/family, not one box | `(9N)MA12345MAX1900` |
| `15` | AIC | Italy's own opaque product code | `(9N)1503222012818` |
| `17` | AIM | Portugal's own opaque product code | `(9N)17583412704` |

Want the exact lengths, character sets, and checksum math behind each of these? See
[`UDI_DI_Structure_And_Fields.md`](UDI_DI_Structure_And_Fields.md).
