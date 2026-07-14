using IfaUdi.Parser.Models;

namespace IfaUdi.Parser.Tests;

public class UdiBuilderTests
{
    private static readonly FixtureFile Fixtures2 = Fixtures.Load();

    public static IEnumerable<object[]> BuildCases() =>
        Fixtures2.BuildFixtures.Select(f => new object[] { f });

    [Theory]
    [MemberData(nameof(BuildCases))]
    public void BuildUdi_MatchesExpectedOutputOrThrows(BuildFixture fixture)
    {
        BuildUdiInput input = ToBuildUdiInput(fixture.Input);
        EnvelopeForm envelopeForm = ToEnvelopeForm(fixture.EnvelopeForm);

        if (!fixture.ExpectedValid)
        {
            IfaUdiBuildException ex = Assert.Throws<IfaUdiBuildException>(() => UdiBuilder.BuildUdi(input, envelopeForm));
            Assert.Equal(fixture.ExpectedReason, ex.Reason);
            return;
        }

        string output = UdiBuilder.BuildUdi(input, envelopeForm);
        Assert.Equal(fixture.ExpectedOutput, output);
    }

    [Theory]
    [MemberData(nameof(BuildCases))]
    public void BuildUdi_RoundTripsThroughParseUdi(BuildFixture fixture)
    {
        if (!fixture.ExpectedValid)
        {
            return;
        }

        BuildUdiInput input = ToBuildUdiInput(fixture.Input);
        EnvelopeForm envelopeForm = ToEnvelopeForm(fixture.EnvelopeForm);

        string output = UdiBuilder.BuildUdi(input, envelopeForm);
        ParsedUdi parsed = UdiParser.ParseUdi(output);

        switch (input.UdiDi.Scheme)
        {
            case UdiScheme.Ppn:
                Assert.Equal(UdiScheme.Ppn, parsed.UdiDi.Scheme);
                Assert.Equal(input.UdiDi.PznBase, parsed.UdiDi.Pzn?[..7]);
                break;
            case UdiScheme.Hpc:
                Assert.Equal(UdiScheme.Hpc, parsed.UdiDi.Scheme);
                Assert.Equal(input.UdiDi.Cin, parsed.UdiDi.Cin);
                Assert.Equal(input.UdiDi.ItemReference, parsed.UdiDi.ItemReference);
                Assert.Equal(input.UdiDi.PackagingLevelIndex, parsed.UdiDi.PackagingLevelIndex);
                break;
            case UdiScheme.MasterUdiDi:
                Assert.Equal(UdiScheme.MasterUdiDi, parsed.UdiDi.Scheme);
                Assert.Equal(input.UdiDi.Cin, parsed.UdiDi.Cin);
                Assert.Equal(input.UdiDi.DeviceGroupCode, parsed.UdiDi.DeviceGroupCode);
                break;
        }

        if (input.UdiPi is not null)
        {
            Assert.Equal(input.UdiPi.Lot, parsed.UdiPi.Lot);
            Assert.Equal(input.UdiPi.ExpiryDate, parsed.UdiPi.ExpiryDate);
            Assert.Equal(input.UdiPi.ManufacturingDate, parsed.UdiPi.ManufacturingDate);
            Assert.Equal(input.UdiPi.SerialNumber, parsed.UdiPi.SerialNumber);
            Assert.Equal(input.UdiPi.Quantity, parsed.UdiPi.Quantity);
            Assert.Equal(input.UdiPi.Price, parsed.UdiPi.Price);
            Assert.Equal(input.UdiPi.Url, parsed.UdiPi.Url);
            Assert.Equal(input.UdiPi.AdditionalGtins, parsed.UdiPi.AdditionalGtins);
        }
    }

    [Fact]
    public void BuildUdi_DefaultsToInterpretationLine()
    {
        var input = new BuildUdiInput { UdiDi = new BuildUdiDiInput { Scheme = UdiScheme.Ppn, PznBase = "1234567" } };
        Assert.Equal("(9N)111234567842", UdiBuilder.BuildUdi(input));
    }

    [Fact]
    public void BuildUdi_RejectsDin16598ForNonHpcScheme()
    {
        var input = new BuildUdiInput { UdiDi = new BuildUdiDiInput { Scheme = UdiScheme.Ppn, PznBase = "1234567" } };
        Assert.Throws<IfaUdiBuildException>(() => UdiBuilder.BuildUdi(input, EnvelopeForm.Din16598));
    }

    private static BuildUdiInput ToBuildUdiInput(BuildFixtureInput input) => new()
    {
        UdiDi = new BuildUdiDiInput
        {
            Scheme = ToUdiScheme(input.UdiDi.Scheme),
            PznBase = input.UdiDi.PznBase,
            Cin = input.UdiDi.Cin,
            ItemReference = input.UdiDi.ItemReference,
            PackagingLevelIndex = input.UdiDi.PackagingLevelIndex,
            DeviceGroupCode = input.UdiDi.DeviceGroupCode,
        },
        UdiPi = input.UdiPi is null ? null : new BuildUdiPiInput
        {
            Lot = input.UdiPi.Lot,
            ExpiryDate = input.UdiPi.ExpiryDate,
            ManufacturingDate = input.UdiPi.ManufacturingDate,
            SerialNumber = input.UdiPi.SerialNumber,
            Quantity = input.UdiPi.Quantity,
            Price = input.UdiPi.Price,
            Url = input.UdiPi.Url,
            AdditionalGtins = input.UdiPi.AdditionalGtins,
        },
    };

    private static UdiScheme ToUdiScheme(string scheme) => scheme switch
    {
        "PPN" => UdiScheme.Ppn,
        "HPC" => UdiScheme.Hpc,
        "MASTER_UDI_DI" => UdiScheme.MasterUdiDi,
        _ => throw new ArgumentOutOfRangeException(nameof(scheme)),
    };

    private static EnvelopeForm ToEnvelopeForm(string? form) => form switch
    {
        null => EnvelopeForm.InterpretationLine,
        "interpretationLine" => EnvelopeForm.InterpretationLine,
        "rawIso15434" => EnvelopeForm.RawIso15434,
        "din16598" => EnvelopeForm.Din16598,
        _ => throw new ArgumentOutOfRangeException(nameof(form)),
    };
}
