using System.Text.RegularExpressions;

namespace IfaUdi.Parser;

/// <summary>
/// Detects and normalizes the three accepted IFA UDI barcode envelope forms (raw ISO/IEC 15434
/// Format 06, DIN 16598 keyboard-compatible, and the printable Interpretation Line) into an
/// ordered list of (DataIdentifier, Value) pairs. See documentation/IFA_UDI_Parser_Analysis.md
/// section 4.
/// </summary>
internal static class Envelope
{
    private const char RecordSeparator = (char)0x1E;
    private const char GroupSeparator = (char)0x1D;
    private const char EndOfTransmission = (char)0x04;

    // Known Data Identifiers, longest first so prefix matching is unambiguous.
    private static readonly string[] KnownDataIdentifiers =
        ["27Q", "16D", "33L", "9N", "1T", "8P", "D", "S", "Q"];

    private static readonly Regex InterpretationLineFieldPattern = new(@"\(([0-9A-Za-z]+)\)([^()]*)", RegexOptions.Compiled);

    public static List<(string Di, string Value)> Normalize(string barcode)
    {
        if (string.IsNullOrEmpty(barcode))
        {
            throw new IfaUdiFormatException("Barcode input must not be empty.");
        }

        if (barcode.StartsWith("[)>", StringComparison.Ordinal))
        {
            return ParseRawIso15434(barcode);
        }

        if (barcode.StartsWith('.') && barcode.Contains('^'))
        {
            return ParseDin16598(barcode);
        }

        if (barcode.StartsWith('(') && InterpretationLineFieldPattern.IsMatch(barcode))
        {
            return ParseInterpretationLine(barcode);
        }

        throw new IfaUdiFormatException("Barcode does not match any recognized IFA UDI envelope form (raw ISO/IEC 15434, DIN 16598, or Interpretation Line).");
    }

    private static List<(string Di, string Value)> ParseRawIso15434(string barcode)
    {
        string rest = barcode[3..]; // strip "[)>"

        if (rest.Length > 0 && rest[0] == RecordSeparator)
        {
            rest = rest[1..];
        }

        if (!rest.StartsWith("06", StringComparison.Ordinal))
        {
            throw new IfaUdiFormatException("Raw ISO/IEC 15434 envelope must use Format 06.");
        }

        rest = rest[2..];

        if (rest.Length > 0 && rest[0] == GroupSeparator)
        {
            rest = rest[1..];
        }

        rest = rest.TrimEnd(RecordSeparator, EndOfTransmission);

        string[] rawFields = rest.Split(GroupSeparator);
        return SplitDataIdentifiers(rawFields);
    }

    private static List<(string Di, string Value)> ParseDin16598(string barcode)
    {
        string rest = barcode[1..]; // strip leading '.'
        string[] rawFields = rest.Split('^');
        return SplitDataIdentifiers(rawFields);
    }

    private static List<(string Di, string Value)> ParseInterpretationLine(string barcode)
    {
        var result = new List<(string, string)>();
        foreach (Match match in InterpretationLineFieldPattern.Matches(barcode))
        {
            result.Add((match.Groups[1].Value, match.Groups[2].Value));
        }

        return result;
    }

    private static List<(string Di, string Value)> SplitDataIdentifiers(string[] rawFields)
    {
        var result = new List<(string, string)>();
        foreach (string field in rawFields)
        {
            string? di = KnownDataIdentifiers.FirstOrDefault(field.StartsWith);
            if (di is null)
            {
                throw new IfaUdiFormatException($"Field '{field}' does not start with a recognized Data Identifier.");
            }

            result.Add((di, field[di.Length..]));
        }

        return result;
    }
}
