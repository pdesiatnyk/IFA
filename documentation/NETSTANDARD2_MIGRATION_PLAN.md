# Migration Plan: `IfaUdi.Parser` → .NET Standard 2.0 / C# 7.3

## Scope

Only **`IfaUdi.Parser`** (the core library) moves to `netstandard2.0` + `LangVersion 7.3`. This is
what makes the parser consumable from .NET Framework 4.6.1+, .NET Core 2.0+, and modern .NET side
by side.

`IfaUdi.Parser.Api` and `IfaUdi.Parser.Tests` **stay on `net8.0`**:

- `IfaUdi.Parser.Api` is an ASP.NET Core Minimal API host (`WebApplication.CreateBuilder`).
  `netstandard2.0` is a library contract, not a runnable TFM — there is no Kestrel/hosting model
  for it. It will keep referencing the library via `ProjectReference`; a `net8.0` app consuming a
  `netstandard2.0` library is the normal, supported direction.
- `IfaUdi.Parser.Tests` stays on `net8.0` so xunit keeps running on the current SDK. Optionally
  (see Phase 4) add a second, older TFM to the test project to prove the library also *runs*
  against an old runtime, not just compiles against the old surface area.

This is a one-way ratchet on the library: everything added to `IfaUdi.Parser` from now on must
stay inside the netstandard2.0/C#7.3 surface. `Api` and `Tests` are unaffected and can keep using
modern C#.

## Why this is not a small change

The library currently uses a cluster of language/BCL features that don't exist in C# 7.3 or in the
netstandard2.0 BCL. This isn't a TFM string edit — every model and most methods need rewriting.

| Feature in use today | Where | Why it breaks on C# 7.3 / netstandard2.0 |
|---|---|---|
| File-scoped namespaces (`namespace X;`) | every file | C# 10 syntax |
| `record` types | `ParsedUdi`, `UdiDi`, `UdiPi`, `BuildUdiInput`, etc. | C# 9 |
| `required` members | all model properties | C# 11 |
| `init` accessors | all model properties | C# 9 |
| Nullable reference types (`string?`) | all models, method signatures | C# 8 (nullable annotation context) |
| Collection expressions (`[]`, `["a","b"]`) | `Envelope.KnownDataIdentifiers`, `additionalGtins ??= []`, `pi.AdditionalGtins ?? []` | C# 12 |
| Range/index operators (`x[..10]`, `x[7..]`, `x[^2]`) | `UdiParser`, `UdiBuilder`, `Envelope` | C# 8 |
| Relational/logical patterns (`is < 1 or > 8`) | `UdiParser`, `UdiBuilder` | C# 9 |
| Switch expressions (`x switch { ... }`) | `UdiBuilder.BuildUdiDi`, `Envelope.Serialize` | C# 8 |
| `char.IsAsciiDigit` | `UdiParser`, `CheckDigits` | .NET 7+ BCL API, not in netstandard2.0 |
| Target-typed `new` (`new()`), tuple deconstruction in `foreach` | throughout | tuples/`new()` are fine in 7.3 in most forms already used; verify case-by-case |

None of this is exotic — it's just that the library was written with the full modern C#/.NET
feature set in mind. The rewrite is mechanical but touches nearly every file.

## Phases

### Phase 0 — Guardrails
- Add a `Directory.Build.props` (or set directly in the `.csproj`) pinning
  `<LangVersion>7.3</LangVersion>` and `<Nullable>disable</Nullable>` for `IfaUdi.Parser` only,
  *before* touching any source. This turns every incompatible construct into a compiler error so
  nothing is missed.
- Do this work on a branch; the library's public shape (namespaces, type names, method
  signatures) should stay the same so `Api` and `Tests` don't need call-site changes beyond what's
  forced by the model rewrite (see Phase 2 note on nullable annotations).

### Phase 1 — Project file
`csharp/IfaUdi.Parser/IfaUdi.Parser.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <LangVersion>7.3</LangVersion>
    <Nullable>disable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
  </PropertyGroup>
</Project>
```
- `ImplicitUsings` must go — the generated `global using` directives require C# 10.
- Delete `obj/`/`bin/` after switching TFMs (stale `net8.0`-targeted intermediate files).

### Phase 2 — Rewrite the models (`Models/*.cs`) — **as implemented**
Convert every `record` with `required`/`init` properties to a plain `sealed class` with public
`{ get; set; }` auto-properties (not the constructor-enforced pattern originally sketched here).
This was a deliberate choice made once implementation started: `IfaUdi.Parser.Tests` (out of
scope, stays on `net8.0`) constructs these types via object-initializer syntax and deliberately
omits optional fields, e.g. `new BuildUdiDiInput { Scheme = UdiScheme.Ppn, PznBase = "1234567" }`
in `UdiBuilderTests.cs`. Object initializers require public setters, not a constructor — a
constructor-enforced immutable shape would have forced changes to the (out-of-scope) Tests
project. Settable properties keep every existing call site — in `UdiParser.cs` and in Tests —
compiling unchanged, and are equally compatible with `System.Text.Json` deserialization in the Api
project (parameterless-constructor-plus-setters is the most broadly-compatible JSON binding shape,
if anything more so than constructor matching).

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
}
```
- No `?` nullable annotations — just reference types, with XML doc comments (already present)
  carrying the "may be null" contract instead of the compiler.
- Records also gave free `Equals`/`GetHashCode`/`ToString`/`with`-expressions. **Checked**:
  `UdiParserTests.cs` and `UdiBuilderTests.cs` only ever assert per-field (`Assert.Equal(expected.UdiDi.Pzn, actual.UdiDi.Pzn)`,
  etc.), never whole-object equality (`Assert.Equal(expectedParsedUdi, actualParsedUdi)`). No
  hand-rolled `Equals`/`GetHashCode` was required for tests to keep passing.
- `BuildUdiDiInput` / `BuildUdiPiInput` / `BuildUdiInput` follow the same pattern.
- Convert `namespace IfaUdi.Parser.Models;` (file-scoped) → `namespace IfaUdi.Parser.Models { ... }`
  (block-scoped) everywhere.

### Phase 3 — Rewrite the logic files

**Two BCL surface gaps only surfaced once the build was actually attempted** (not predictable from
language-version analysis alone, since they're netstandard2.0 API-surface gaps, not C# syntax
gaps):
- `string.Join(char, IEnumerable<string>)` and the `char`-separator overload generally — added in
  .NET Core 2.1 / netstandard2.1, **not present in netstandard2.0**. `Envelope.Serialize` used
  `string.Join(GroupSeparator, ...)` (a `char` constant) and `string.Join('^', ...)` — both needed
  to become `string.Join(GroupSeparator.ToString(), ...)` / `string.Join("^", ...)`.
- `string.StartsWith(char)` / `string.EndsWith(char)` — availability on netstandard2.0 was
  uncertain enough that `Envelope.cs`'s `barcode.StartsWith('.')` / `StartsWith('(')` were replaced
  with plain index checks (`barcode[0] == '.'`) rather than relying on an overload that might not
  resolve; `barcode` is already known non-empty at that point (guarded by an earlier
  `IsNullOrEmpty` check), so the direct index is safe.

- **`UdiParser.cs`**
  - Replace all range/index operators: `value[..10]` → `value.Substring(0, 10)`,
    `pi[..7]` → `pi.Substring(0, 7)`, `value[^2]` → `value.Substring(value.Length - 2)`, etc.
  - Replace `char.IsAsciiDigit` → `c is >= '0' and <= '9'` is *also* a C# 9 pattern — use
    `c >= '0' && c <= '9'`, or `char.IsDigit(c)` if full-Unicode-digit is acceptable (it isn't
    quite equivalent — `char.IsDigit` accepts non-ASCII decimal digits — so prefer the explicit
    range check to preserve current ASCII-only semantics).
  - Replace relational patterns: `pli is < '0' or > '8'` → `pli < '0' || pli > '8'`;
    `value.Length is < 1 or > 8` → `value.Length < 1 || value.Length > 8` (repeat throughout
    `UdiParser`/`UdiBuilder`).
  - `switch` statements (already statement form, not expression) are fine as-is in `UdiParser`.
- **`UdiBuilder.cs`**
  - `BuildUdiDi`'s `switch` **expression** (`input.Scheme switch { ... }`) → convert to a
    `switch` **statement** with local variable assignment, or an if/else chain.
  - Same range-operator and relational-pattern replacements as above (`year[2..]`, `is < 1 or > 8`,
    etc.).
  - `pi.AdditionalGtins ?? []` → `pi.AdditionalGtins ?? new List<string>()` (or `Enumerable.Empty`
    if only enumerated).
  - `additionalGtins ??= []` in `UdiParser.cs` needed two fixes at once: the compound
    null-coalescing assignment `??=` is itself C# 8 (only the *simple* `??` operator predates it),
    and its right-hand side used the collection expression. Replaced with an explicit
    `if (additionalGtins is null) { additionalGtins = new List<string>(); }`.
  - `new(...)` target-typed object creation (`Regex DateOnlyPattern = new(@"...", RegexOptions.Compiled)`)
    is a C# 9 feature and appears throughout `Validation.cs`, `UdiBuilder.cs`, and `Envelope.cs`'s
    `Regex` field initializers — all rewritten to explicit `new Regex(...)`.
- **`Envelope.cs`**
  - `KnownDataIdentifiers = ["27Q", "16D", ...]` (collection expression) →
    `new[] { "27Q", "16D", "33L", "9N", "1T", "8P", "D", "S", "Q" }`.
  - `Serialize`'s `switch` expression → `switch` statement or if/else.
  - Range operators (`barcode[3..]`, `rest[1..]`, `rest[2..]`) → `Substring` equivalents.
  - `barcode.StartsWith('.')` / `StartsWith('(')` → `barcode[0] == '.'` / `barcode[0] == '('`
    (see BCL-surface-gap note above).
- **`CheckDigits.cs`**
  - `sevenDigitBase.All(char.IsAsciiDigit)` → `sevenDigitBase.All(c => c >= '0' && c <= '9')`.
- **`Validation.cs`**, **`IfaUdiFormatException.cs`**, **`IfaUdiBuildException.cs`**
  - Only need the namespace-brace conversion; no other blocked features found.
- Global find/replace across the project: `namespace X;` → `namespace X { ... }` block form (must
  be done file-by-file since it changes brace structure, not a blind text substitution).

### Phase 4 — Verify the library actually runs on an old runtime
Compiling against `netstandard2.0` only proves the *API surface* is old enough — it doesn't prove
the library runs correctly under .NET Framework's implementation of that surface (e.g. regex
behavior, string comparison culture defaults).

**Attempted, inconclusive in this environment**: temporarily multi-targeted `IfaUdi.Parser.Tests`
to `net8.0;net472` and ran `dotnet test -f net472`. This immediately hit compile errors that belong
to the *Tests project itself*, not the library: `Fixtures.cs` uses `System.Text.Json` (not part of
the net472 BCL — would need a `System.Text.Json` PackageReference) and C# 9 `record`/`init` types
(needs an `IsExternalInit` polyfill type to compile on net472, since the BCL doesn't ship one).
Both are fixable, but they're shims for the **out-of-scope** Tests project, not evidence about the
migrated library — so the experiment was reverted (`IfaUdi.Parser.Tests.csproj` back to
`net8.0`-only) rather than spending scope on it. The `IfaUdi.Parser` library itself has zero
netstandard2.0-incompatible constructs (confirmed by a clean, warning-free standalone build), so
there's no specific reason to expect a real .NET Framework consumer to behave differently — this
just wasn't independently proven end-to-end in this environment. If it matters later, the cleanest
path is a small standalone net472 console/xunit harness that references only `IfaUdi.Parser.dll`
directly, instead of multi-targeting the existing Tests project.

### Phase 5 — Api and Tests wiring
- No TFM changes needed for `Api`/`Tests` — `ProjectReference` to a `netstandard2.0` library from
  a `net8.0` project works unmodified.
- `Api`'s `Program.cs` and `JsonConverters.cs` reference `UdiScheme`, `EnvelopeForm`, `UdiDi`,
  etc. Once these are plain classes instead of records, check:
  - Any `with`-expression usage (none found in current `Api`/`Tests` source).
  - Any pattern-matching against record positional deconstruction (none found — all access is via
    named properties, which keeps working).
- Minimal API request records in `Program.cs` (`CheckRequest`, `ParseRequest`, `BuildRequest`) stay
  as `net8.0`/C# 12 records — they're in the `Api` project, out of scope.

### Phase 6 — Validate — **done**
- `dotnet build csharp/IfaUdi.Parser.sln` — full solution builds clean: library on
  `netstandard2.0`/C# 7.3, Api and Tests unchanged on `net8.0`. 0 warnings, 0 errors.
- `dotnet test csharp/IfaUdi.Parser.Tests` — 52/52 tests pass, including the
  `shared-fixtures/udi-test-cases.json` data-driven tests in `Fixtures.cs`. No test assertions or
  fixtures needed to change.
- Public API surface (namespaces, type names, method signatures) is unchanged — only the internal
  representation (`record` → plain class with settable properties) changed, so `Api` and `Tests`
  needed zero source changes to keep compiling and passing.
- Manual smoke-test of the running `Api` — **done**. Ran `dotnet run` and hit all endpoints with
  `curl`: `/health`, `/api/check` (valid + invalid barcode), `/api/parse` (valid + invalid,
  including the null-field JSON shape from the rewritten model classes), `/api/build` (PPN
  defaults, HPC with UDI-PI fields, and a missing-required-field error case). All responses
  matched expected pre-migration behavior exactly.

## Resolved decisions
1. **Runtime floor**: no specific .NET Framework version to target beyond netstandard2.0's own
   floor (4.6.1) — the goal is broad compatibility in general, not a pinned minimum. Phase 4's
   old-runtime check is a one-time sanity pass, not an ongoing multi-targeted test leg.
2. **Nullability intent**: preserved via the existing XML doc comments only (many already note
   e.g. "PPN only", "HPC only" per-field). No `[CanBeNull]`/`[NotNull]`-style attributes or other
   annotation tooling added.
