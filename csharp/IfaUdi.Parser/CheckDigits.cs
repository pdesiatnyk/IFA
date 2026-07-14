namespace IfaUdi.Parser;

/// <summary>
/// Check-digit algorithms per documentation/IFA_UDI_Parser_Analysis.md section 5.
/// </summary>
public static class CheckDigits
{
    /// <summary>
    /// Modulo-97 check digit shared by PPN, HPC, Master UDI-DI and Basic UDI-DI.
    /// <paramref name="payload"/> must be the full value including the PRA-Code/IAC prefix
    /// but excluding the trailing 2 check-digit characters.
    /// </summary>
    public static string Mod97(string payload)
    {
        long sum = 0;
        int weight = 2;
        foreach (char c in payload)
        {
            sum += (long)c * weight;
            weight++;
        }

        int remainder = (int)(sum % 97);
        return remainder.ToString("D2");
    }

    /// <summary>
    /// Modulo-11 check digit for the 7-digit PZN base. Throws if the computed remainder is 10,
    /// which per spec is never issued as a PZN.
    /// </summary>
    public static char Mod11Pzn(string sevenDigitBase)
    {
        if (sevenDigitBase.Length != 7 || !sevenDigitBase.All(char.IsAsciiDigit))
        {
            throw new IfaUdiFormatException($"PZN base must be exactly 7 digits, got '{sevenDigitBase}'.");
        }

        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            int digit = sevenDigitBase[i] - '0';
            sum += digit * (i + 1);
        }

        int remainder = sum % 11;
        if (remainder == 10)
        {
            throw new IfaUdiFormatException("Computed PZN check digit remainder is 10; this digit sequence was never issued as a PZN.");
        }

        return (char)('0' + remainder);
    }
}
