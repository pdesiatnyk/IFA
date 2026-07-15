using IfaUdi.Parser.Models;

namespace IfaUdi.Parser.Tests;

public class UdiParserTests
{
    private static readonly FixtureFile Fixtures2 = Fixtures.Load();

    public static IEnumerable<object[]> BarcodeCases() =>
        Fixtures2.BarcodeFixtures.Select(f => new object[] { f });

    [Theory]
    [MemberData(nameof(BarcodeCases))]
    public void Check_MatchesExpectedValidity(BarcodeFixture fixture)
    {
        Assert.Equal(fixture.ExpectedValid, UdiParser.Check(fixture.Input));
    }

    [Theory]
    [MemberData(nameof(BarcodeCases))]
    public void ParseUdi_MatchesExpectedStructureOrThrows(BarcodeFixture fixture)
    {
        if (!fixture.ExpectedValid)
        {
            Assert.Throws<IfaUdiFormatException>(() => UdiParser.ParseUdi(fixture.Input));
            return;
        }

        ParsedUdi actual = UdiParser.ParseUdi(fixture.Input);
        ExpectedParsedUdi expected = fixture.Expected!;

        Assert.Equal(expected.UdiDi.Raw, actual.UdiDi.Raw);
        Assert.Equal(expected.UdiDi.Scheme, SchemeToString(actual.UdiDi.Scheme));
        Assert.Equal(expected.UdiDi.PraCode, actual.UdiDi.PraCode);
        Assert.Equal(expected.UdiDi.Pzn, actual.UdiDi.Pzn);
        Assert.Equal(expected.UdiDi.Cin, actual.UdiDi.Cin);
        Assert.Equal(expected.UdiDi.ItemReference, actual.UdiDi.ItemReference);
        Assert.Equal(expected.UdiDi.PackagingLevelIndex, actual.UdiDi.PackagingLevelIndex);
        Assert.Equal(expected.UdiDi.DeviceGroupCode, actual.UdiDi.DeviceGroupCode);
        Assert.Equal(expected.UdiDi.NationalCode, actual.UdiDi.NationalCode);
        Assert.Equal(expected.UdiDi.CheckDigits, actual.UdiDi.CheckDigits);

        Assert.Equal(expected.UdiPi.Lot, actual.UdiPi.Lot);
        Assert.Equal(expected.UdiPi.ExpiryDate, actual.UdiPi.ExpiryDate);
        Assert.Equal(expected.UdiPi.ManufacturingDate, actual.UdiPi.ManufacturingDate);
        Assert.Equal(expected.UdiPi.SerialNumber, actual.UdiPi.SerialNumber);
        Assert.Equal(expected.UdiPi.Quantity, actual.UdiPi.Quantity);
        Assert.Equal(expected.UdiPi.Price, actual.UdiPi.Price);
        Assert.Equal(expected.UdiPi.Url, actual.UdiPi.Url);
        Assert.Equal(expected.UdiPi.AdditionalGtins, actual.UdiPi.AdditionalGtins);
    }

    private static string SchemeToString(UdiScheme scheme) => scheme switch
    {
        UdiScheme.Ppn => "PPN",
        UdiScheme.Hpc => "HPC",
        UdiScheme.MasterUdiDi => "MASTER_UDI_DI",
        UdiScheme.Aic => "AIC",
        UdiScheme.Aim => "AIM",
        _ => throw new ArgumentOutOfRangeException(nameof(scheme)),
    };
}
