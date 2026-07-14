using System.Text.RegularExpressions;
using IfaUdi.Parser.Models;

namespace IfaUdi.Parser;

public static class UdiParser
{
    public static bool Check(string barcode)
    {
        try
        {
            ParseUdi(barcode);
            return true;
        }
        catch (IfaUdiFormatException)
        {
            return false;
        }
    }

    public static ParsedUdi ParseUdi(string barcode)
    {
        List<(string Di, string Value)> fields = Envelope.Normalize(barcode);

        List<(string Di, string Value)> udiDiFields = fields.Where(f => f.Di == "9N").ToList();
        if (udiDiFields.Count != 1)
        {
            throw new IfaUdiFormatException($"Expected exactly one UDI-DI (Data Identifier '9N') field, found {udiDiFields.Count}.");
        }

        UdiDi udiDi = ParseUdiDi(udiDiFields[0].Value);
        UdiPi udiPi = ParseUdiPi(fields.Where(f => f.Di != "9N"));

        return new ParsedUdi { UdiDi = udiDi, UdiPi = udiPi };
    }

    private static UdiDi ParseUdiDi(string value)
    {
        if (value.StartsWith("11", StringComparison.Ordinal))
        {
            return ParsePpn(value);
        }

        if (value.StartsWith("13", StringComparison.Ordinal))
        {
            return ParseHpc(value);
        }

        if (value.StartsWith("MA", StringComparison.Ordinal))
        {
            return ParseMasterUdiDi(value);
        }

        throw new IfaUdiFormatException($"UDI-DI value '{value}' does not start with a supported PRA-Code (11, 13, or MA).");
    }

    private static UdiDi ParsePpn(string value)
    {
        if (value.Length != 12)
        {
            throw new IfaUdiFormatException($"PPN value must be exactly 12 characters, got {value.Length}.");
        }

        string pzn = value.Substring(2, 8);
        string checkDigits = value.Substring(10, 2);

        if (!pzn.All(char.IsAsciiDigit))
        {
            throw new IfaUdiFormatException($"PZN '{pzn}' must be 8 digits.");
        }

        string expectedCheck = CheckDigits.Mod97(value[..10]);
        if (expectedCheck != checkDigits)
        {
            throw new IfaUdiFormatException($"PPN check digits mismatch: expected '{expectedCheck}', got '{checkDigits}'.");
        }

        char expectedPznCheck = CheckDigits.Mod11Pzn(pzn[..7]);
        if (expectedPznCheck != pzn[7])
        {
            throw new IfaUdiFormatException($"PZN check digit mismatch: expected '{expectedPznCheck}', got '{pzn[7]}'.");
        }

        return new UdiDi
        {
            Raw = value,
            Scheme = UdiScheme.Ppn,
            PraCode = "11",
            Pzn = pzn,
            CheckDigits = checkDigits,
        };
    }

    private static UdiDi ParseHpc(string value)
    {
        if (value.Length < 11 || value.Length > 28)
        {
            throw new IfaUdiFormatException($"HPC value must be 11-28 characters, got {value.Length}.");
        }

        string cin = value.Substring(2, 5);
        int itemReferenceLength = value.Length - 10;
        string itemReference = value.Substring(7, itemReferenceLength);
        char pli = value[7 + itemReferenceLength];
        string checkDigits = value.Substring(8 + itemReferenceLength, 2);

        if (!Validation.AlphanumericUpperCharset.IsMatch(cin))
        {
            throw new IfaUdiFormatException($"HPC CIN '{cin}' must be alphanumeric uppercase.");
        }

        if (!Validation.ItemReferenceCharset.IsMatch(itemReference))
        {
            throw new IfaUdiFormatException($"HPC item reference '{itemReference}' contains characters outside 0-9, A-Z, '.', '-'.");
        }

        if (pli is < '0' or > '8')
        {
            throw new IfaUdiFormatException($"HPC packaging level index '{pli}' is invalid; only 0-8 are valid for UDI (9 is reserved for variable quantities).");
        }

        string expectedCheck = CheckDigits.Mod97(value[..^2]);
        if (expectedCheck != checkDigits)
        {
            throw new IfaUdiFormatException($"HPC check digits mismatch: expected '{expectedCheck}', got '{checkDigits}'.");
        }

        return new UdiDi
        {
            Raw = value,
            Scheme = UdiScheme.Hpc,
            PraCode = "13",
            Cin = cin,
            ItemReference = itemReference,
            PackagingLevelIndex = pli - '0',
            CheckDigits = checkDigits,
        };
    }

    private static UdiDi ParseMasterUdiDi(string value)
    {
        if (value.Length < 10 || value.Length > 28)
        {
            throw new IfaUdiFormatException($"Master UDI-DI value must be 10-28 characters, got {value.Length}.");
        }

        string cin = value.Substring(2, 5);
        int deviceGroupLength = value.Length - 9;
        string deviceGroupCode = value.Substring(7, deviceGroupLength);
        string checkDigits = value.Substring(7 + deviceGroupLength, 2);

        if (!Validation.AlphanumericUpperCharset.IsMatch(cin))
        {
            throw new IfaUdiFormatException($"Master UDI-DI CIN '{cin}' must be alphanumeric uppercase.");
        }

        if (!Validation.ItemReferenceCharset.IsMatch(deviceGroupCode))
        {
            throw new IfaUdiFormatException($"Master UDI-DI device group code '{deviceGroupCode}' contains characters outside 0-9, A-Z, '.', '-'.");
        }

        string expectedCheck = CheckDigits.Mod97(value[..^2]);
        if (expectedCheck != checkDigits)
        {
            throw new IfaUdiFormatException($"Master UDI-DI check digits mismatch: expected '{expectedCheck}', got '{checkDigits}'.");
        }

        return new UdiDi
        {
            Raw = value,
            Scheme = UdiScheme.MasterUdiDi,
            PraCode = "MA",
            Cin = cin,
            DeviceGroupCode = deviceGroupCode,
            CheckDigits = checkDigits,
        };
    }

    private static UdiPi ParseUdiPi(IEnumerable<(string Di, string Value)> fields)
    {
        string? lot = null;
        string? expiryDate = null;
        string? manufacturingDate = null;
        string? serialNumber = null;
        int? quantity = null;
        string? price = null;
        string? url = null;
        List<string>? additionalGtins = null;

        foreach ((string di, string value) in fields)
        {
            switch (di)
            {
                case "1T":
                    lot = ValidateLotOrSerial(value, "LOT");
                    break;
                case "D":
                    expiryDate = ParseDate(value, expectFourDigitYear: false);
                    break;
                case "16D":
                    manufacturingDate = ParseDate(value, expectFourDigitYear: true);
                    break;
                case "S":
                    serialNumber = ValidateLotOrSerial(value, "SN");
                    break;
                case "Q":
                    if (value.Length is < 1 or > 8 || !value.All(char.IsAsciiDigit))
                    {
                        throw new IfaUdiFormatException($"Quantity '{value}' must be 1-8 digits.");
                    }

                    quantity = int.Parse(value);
                    break;
                case "27Q":
                    if (value.Length is < 4 or > 20 || !Regex.IsMatch(value, @"^[0-9.]+$"))
                    {
                        throw new IfaUdiFormatException($"Price '{value}' must be 4-20 characters of digits and '.'.");
                    }

                    price = value;
                    break;
                case "33L":
                    url = value;
                    break;
                case "8P":
                    if (value.Length != 14 || !value.All(char.IsAsciiDigit))
                    {
                        throw new IfaUdiFormatException($"Additional GTIN/NTIN '{value}' must be exactly 14 digits.");
                    }

                    additionalGtins ??= [];
                    additionalGtins.Add(value);
                    break;
                default:
                    throw new IfaUdiFormatException($"Unrecognized UDI-PI Data Identifier '{di}'.");
            }
        }

        return new UdiPi
        {
            Lot = lot,
            ExpiryDate = expiryDate,
            ManufacturingDate = manufacturingDate,
            SerialNumber = serialNumber,
            Quantity = quantity,
            Price = price,
            Url = url,
            AdditionalGtins = additionalGtins,
        };
    }

    private static string ValidateLotOrSerial(string value, string fieldName)
    {
        if (value.Length is < 1 or > 20)
        {
            throw new IfaUdiFormatException($"{fieldName} '{value}' must be 1-20 characters.");
        }

        if (Validation.ForbiddenLotSnChars.IsMatch(value))
        {
            throw new IfaUdiFormatException($"{fieldName} '{value}' contains a technically excluded character.");
        }

        return value;
    }

    private static string ParseDate(string value, bool expectFourDigitYear)
    {
        int expectedLength = expectFourDigitYear ? 8 : 6;
        if (value.Length != expectedLength || !value.All(char.IsAsciiDigit))
        {
            throw new IfaUdiFormatException($"Date '{value}' must be exactly {expectedLength} digits ({(expectFourDigitYear ? "YYYYMMDD" : "YYMMDD")}).");
        }

        int year = expectFourDigitYear
            ? int.Parse(value[..4])
            : 2000 + int.Parse(value[..2]);

        int month = int.Parse(value.Substring(expectFourDigitYear ? 4 : 2, 2));
        int day = int.Parse(value.Substring(expectFourDigitYear ? 6 : 4, 2));

        if (month is < 1 or > 12)
        {
            throw new IfaUdiFormatException($"Date '{value}' has invalid month {month:D2}.");
        }

        if (day == 0 && !expectFourDigitYear)
        {
            return $"{year:D4}-{month:D2}";
        }

        if (day is < 1 or > 31)
        {
            throw new IfaUdiFormatException($"Date '{value}' has invalid day {day:D2}.");
        }

        return $"{year:D4}-{month:D2}-{day:D2}";
    }
}
