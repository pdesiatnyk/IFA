namespace IfaUdi.Parser.Tests;

public class CheckDigitTests
{
    private static readonly FixtureFile Fixtures2 = Fixtures.Load();

    public static IEnumerable<object[]> Mod97Cases() =>
        Fixtures2.CheckDigitFixtures["mod97"].Select(f => new object[] { f.Input, f.Expected });

    public static IEnumerable<object[]> Mod11PznCases() =>
        Fixtures2.CheckDigitFixtures["mod11Pzn"].Select(f => new object[] { f.Input, f.Expected });

    [Theory]
    [MemberData(nameof(Mod97Cases))]
    public void Mod97_MatchesFixtures(string input, string expected)
    {
        Assert.Equal(expected, CheckDigits.Mod97(input));
    }

    [Theory]
    [MemberData(nameof(Mod11PznCases))]
    public void Mod11Pzn_MatchesFixtures(string input, string expected)
    {
        Assert.Equal(expected[0], CheckDigits.Mod11Pzn(input));
    }
}
