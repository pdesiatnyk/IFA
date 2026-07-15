using System.Text.Json;

namespace IfaUdi.Parser.Tests;

public sealed record CheckDigitFixture(string Input, string Expected, string? Note);

public sealed record ExpectedUdiDi(
    string Raw,
    string Scheme,
    string PraCode,
    string? Pzn,
    string? Cin,
    string? ItemReference,
    int? PackagingLevelIndex,
    string? DeviceGroupCode,
    string? NationalCode,
    string CheckDigits);

public sealed record ExpectedUdiPi(
    string? Lot,
    string? ExpiryDate,
    string? ManufacturingDate,
    string? SerialNumber,
    int? Quantity,
    string? Price,
    string? Url,
    List<string>? AdditionalGtins);

public sealed record ExpectedParsedUdi(ExpectedUdiDi UdiDi, ExpectedUdiPi UdiPi);

public sealed record BarcodeFixture(
    string Name,
    string Input,
    string InputForm,
    bool ExpectedValid,
    string? Reason,
    string? Note,
    ExpectedParsedUdi? Expected);

public sealed record BuildFixtureUdiDiInput(
    string Scheme,
    string? PznBase,
    string? Cin,
    string? ItemReference,
    int? PackagingLevelIndex,
    string? DeviceGroupCode,
    string? NationalCode);

public sealed record BuildFixtureUdiPiInput(
    string? Lot,
    string? ExpiryDate,
    string? ManufacturingDate,
    string? SerialNumber,
    int? Quantity,
    string? Price,
    string? Url,
    List<string>? AdditionalGtins);

public sealed record BuildFixtureInput(BuildFixtureUdiDiInput UdiDi, BuildFixtureUdiPiInput? UdiPi);

public sealed record BuildFixture(
    string Name,
    BuildFixtureInput Input,
    string? EnvelopeForm,
    bool ExpectedValid,
    string? ExpectedOutput,
    string? ExpectedReason,
    string? Note);

internal sealed record FixtureFile(
    Dictionary<string, List<CheckDigitFixture>> CheckDigitFixtures,
    List<BarcodeFixture> BarcodeFixtures,
    List<BuildFixture> BuildFixtures);

internal static class Fixtures
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static FixtureFile Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "TestFixtures", "udi-test-cases.json");
        string json = File.ReadAllText(path);
        using JsonDocument doc = JsonDocument.Parse(json);

        var checkDigitFixtures = new Dictionary<string, List<CheckDigitFixture>>();
        foreach (JsonProperty prop in doc.RootElement.GetProperty("checkDigitFixtures").EnumerateObject())
        {
            checkDigitFixtures[prop.Name] = prop.Value.Deserialize<List<CheckDigitFixture>>(Options)!;
        }

        var barcodeFixtures = doc.RootElement.GetProperty("barcodeFixtures").Deserialize<List<BarcodeFixture>>(Options)!;
        var buildFixtures = doc.RootElement.GetProperty("buildFixtures").Deserialize<List<BuildFixture>>(Options)!;

        return new FixtureFile(checkDigitFixtures, barcodeFixtures, buildFixtures);
    }
}
