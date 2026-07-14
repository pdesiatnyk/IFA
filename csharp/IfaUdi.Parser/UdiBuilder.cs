using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IfaUdi.Parser.Models;

namespace IfaUdi.Parser
{
    /// <summary>
    /// Constructs a valid IFA UDI barcode string from structured UDI-DI/UDI-PI input, the inverse of
    /// <see cref="UdiParser.ParseUdi"/>. Check digits are always computed, never user-supplied.
    /// Throws <see cref="IfaUdiBuildException"/> on invalid input.
    /// </summary>
    public static class UdiBuilder
    {
        private static readonly Regex DateOnlyPattern = new Regex(@"^([0-9]{4})-([0-9]{2})-([0-9]{2})$", RegexOptions.Compiled);
        private static readonly Regex YearMonthPattern = new Regex(@"^([0-9]{4})-([0-9]{2})$", RegexOptions.Compiled);

        public static string BuildUdi(BuildUdiInput input, EnvelopeForm envelopeForm = EnvelopeForm.InterpretationLine)
        {
            if (envelopeForm == EnvelopeForm.Din16598 && input.UdiDi.Scheme != UdiScheme.Hpc)
            {
                throw new IfaUdiBuildException("DIN 16598 envelope form is only valid for the HPC scheme.", "envelopeForm", "DIN16598_HPC_ONLY");
            }

            var fields = new List<(string Di, string Value)> { ("9N", BuildUdiDi(input.UdiDi)) };
            fields.AddRange(BuildUdiPiFields(input.UdiPi));

            return Envelope.Serialize(fields, envelopeForm);
        }

        private static string BuildUdiDi(BuildUdiDiInput input)
        {
            switch (input.Scheme)
            {
                case UdiScheme.Ppn:
                    return BuildPpn(input.PznBase ?? throw new IfaUdiBuildException("Required for the PPN scheme.", "udiDi.pznBase", "MISSING_FIELD"));
                case UdiScheme.Hpc:
                    return BuildHpc(
                        input.Cin ?? throw new IfaUdiBuildException("Required for the HPC scheme.", "udiDi.cin", "MISSING_FIELD"),
                        input.ItemReference ?? throw new IfaUdiBuildException("Required for the HPC scheme.", "udiDi.itemReference", "MISSING_FIELD"),
                        input.PackagingLevelIndex ?? throw new IfaUdiBuildException("Required for the HPC scheme.", "udiDi.packagingLevelIndex", "MISSING_FIELD"));
                case UdiScheme.MasterUdiDi:
                    return BuildMasterUdiDi(
                        input.Cin ?? throw new IfaUdiBuildException("Required for the Master UDI-DI scheme.", "udiDi.cin", "MISSING_FIELD"),
                        input.DeviceGroupCode ?? throw new IfaUdiBuildException("Required for the Master UDI-DI scheme.", "udiDi.deviceGroupCode", "MISSING_FIELD"));
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        private static string BuildPpn(string pznBase)
        {
            char pznCheckDigit;
            try
            {
                pznCheckDigit = CheckDigits.Mod11Pzn(pznBase);
            }
            catch (IfaUdiFormatException ex)
            {
                throw new IfaUdiBuildException(ex.Message, "udiDi.pznBase", "INVALID_PZN_BASE");
            }

            string value = $"11{pznBase}{pznCheckDigit}"; // "11" + 7-digit base + 1-digit check = 10 chars
            return value + CheckDigits.Mod97(value);
        }

        private static string BuildHpc(string cin, string itemReference, int packagingLevelIndex)
        {
            AssertCin(cin, "udiDi.cin");

            if (itemReference.Length < 1 || itemReference.Length > 18 || !Validation.ItemReferenceCharset.IsMatch(itemReference))
            {
                throw new IfaUdiBuildException("Must be 1-18 characters of 0-9, A-Z, '.' or '-'.", "udiDi.itemReference", "INVALID_ITEM_REFERENCE");
            }

            if (packagingLevelIndex < 0 || packagingLevelIndex > 8)
            {
                throw new IfaUdiBuildException(
                    "Must be an integer 0-8 (9 is reserved for variable quantities, invalid for UDI).",
                    "udiDi.packagingLevelIndex",
                    "INVALID_PACKAGING_LEVEL_INDEX");
            }

            string value = $"13{cin}{itemReference}{packagingLevelIndex}";
            return value + CheckDigits.Mod97(value);
        }

        private static string BuildMasterUdiDi(string cin, string deviceGroupCode)
        {
            AssertCin(cin, "udiDi.cin");

            if (deviceGroupCode.Length < 1 || deviceGroupCode.Length > 19 || !Validation.ItemReferenceCharset.IsMatch(deviceGroupCode))
            {
                throw new IfaUdiBuildException("Must be 1-19 characters of 0-9, A-Z, '.' or '-'.", "udiDi.deviceGroupCode", "INVALID_DEVICE_GROUP_CODE");
            }

            string value = $"MA{cin}{deviceGroupCode}";
            return value + CheckDigits.Mod97(value);
        }

        private static void AssertCin(string cin, string field)
        {
            if (cin.Length != 5 || !Validation.AlphanumericUpperCharset.IsMatch(cin))
            {
                throw new IfaUdiBuildException("Must be exactly 5 alphanumeric uppercase characters.", field, "INVALID_CIN");
            }
        }

        private static List<(string Di, string Value)> BuildUdiPiFields(BuildUdiPiInput pi)
        {
            var fields = new List<(string, string)>();
            if (pi is null)
            {
                return fields;
            }

            if (pi.Lot != null)
            {
                fields.Add(("1T", AssertLotOrSerial(pi.Lot, "udiPi.lot")));
            }

            if (pi.ExpiryDate != null)
            {
                fields.Add(("D", EncodeExpiryDate(pi.ExpiryDate)));
            }

            if (pi.ManufacturingDate != null)
            {
                fields.Add(("16D", EncodeManufacturingDate(pi.ManufacturingDate)));
            }

            if (pi.SerialNumber != null)
            {
                fields.Add(("S", AssertLotOrSerial(pi.SerialNumber, "udiPi.serialNumber")));
            }

            if (pi.Quantity != null)
            {
                fields.Add(("Q", EncodeQuantity(pi.Quantity.Value)));
            }

            if (pi.Price != null)
            {
                fields.Add(("27Q", AssertPrice(pi.Price)));
            }

            if (pi.Url != null)
            {
                fields.Add(("33L", pi.Url));
            }

            foreach (string gtin in pi.AdditionalGtins ?? new List<string>())
            {
                fields.Add(("8P", AssertGtin(gtin)));
            }

            return fields;
        }

        private static string AssertLotOrSerial(string value, string field)
        {
            if (value.Length < 1 || value.Length > 20)
            {
                throw new IfaUdiBuildException("Must be 1-20 characters.", field, "INVALID_LOT_OR_SERIAL");
            }

            if (Validation.ForbiddenLotSnChars.IsMatch(value))
            {
                throw new IfaUdiBuildException("Contains a technically excluded character.", field, "INVALID_LOT_OR_SERIAL");
            }

            return value;
        }

        private static string EncodeExpiryDate(string value)
        {
            Match dateOnly = DateOnlyPattern.Match(value);
            if (dateOnly.Success)
            {
                string year = dateOnly.Groups[1].Value;
                string month = dateOnly.Groups[2].Value;
                string day = dateOnly.Groups[3].Value;
                AssertMonthDay(month, day, "udiPi.expiryDate");
                return $"{year.Substring(2)}{month}{day}";
            }

            Match yearMonth = YearMonthPattern.Match(value);
            if (yearMonth.Success)
            {
                string year = yearMonth.Groups[1].Value;
                string month = yearMonth.Groups[2].Value;
                AssertMonthDay(month, "01", "udiPi.expiryDate");
                return $"{year.Substring(2)}{month}00";
            }

            throw new IfaUdiBuildException("Must be \"YYYY-MM-DD\" or \"YYYY-MM\".", "udiPi.expiryDate", "INVALID_DATE");
        }

        private static string EncodeManufacturingDate(string value)
        {
            Match dateOnly = DateOnlyPattern.Match(value);
            if (!dateOnly.Success)
            {
                throw new IfaUdiBuildException("Must be \"YYYY-MM-DD\".", "udiPi.manufacturingDate", "INVALID_DATE");
            }

            string year = dateOnly.Groups[1].Value;
            string month = dateOnly.Groups[2].Value;
            string day = dateOnly.Groups[3].Value;
            AssertMonthDay(month, day, "udiPi.manufacturingDate");
            return $"{year}{month}{day}";
        }

        private static void AssertMonthDay(string month, string day, string field)
        {
            int monthNum = int.Parse(month);
            int dayNum = int.Parse(day);
            if (monthNum < 1 || monthNum > 12)
            {
                throw new IfaUdiBuildException($"Invalid month {month}.", field, "MONTH_OUT_OF_RANGE");
            }

            if (dayNum < 1 || dayNum > 31)
            {
                throw new IfaUdiBuildException($"Invalid day {day}.", field, "DAY_OUT_OF_RANGE");
            }
        }

        private static string EncodeQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new IfaUdiBuildException("Must be a non-negative integer.", "udiPi.quantity", "INVALID_QUANTITY");
            }

            string value = quantity.ToString();
            if (value.Length < 1 || value.Length > 8)
            {
                throw new IfaUdiBuildException("Must be representable in 1-8 digits.", "udiPi.quantity", "INVALID_QUANTITY");
            }

            return value;
        }

        private static string AssertPrice(string value)
        {
            if (value.Length < 4 || value.Length > 20 || !Regex.IsMatch(value, @"^[0-9.]+$"))
            {
                throw new IfaUdiBuildException("Must be 4-20 characters of digits and '.'.", "udiPi.price", "INVALID_PRICE");
            }

            return value;
        }

        private static string AssertGtin(string value)
        {
            if (value.Length != 14 || !value.All(c => c >= '0' && c <= '9'))
            {
                throw new IfaUdiBuildException("Must be exactly 14 digits.", "udiPi.additionalGtins", "INVALID_GTIN");
            }

            return value;
        }
    }
}
