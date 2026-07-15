using System.Collections.Generic;

namespace IfaUdi.Parser.Models
{
    public sealed class BuildUdiDiInput
    {
        public UdiScheme Scheme { get; set; }

        /// <summary>PPN only. 7-digit base; the 8th (mod-11) PZN check digit is always computed.</summary>
        public string PznBase { get; set; }

        /// <summary>HPC and Master UDI-DI only. Exactly 5 alphanumeric-uppercase characters.</summary>
        public string Cin { get; set; }

        /// <summary>HPC only. 1-18 chars of 0-9, A-Z, '.', '-'.</summary>
        public string ItemReference { get; set; }

        /// <summary>HPC only. 0-8 (9 is reserved for variable quantities, invalid for UDI).</summary>
        public int? PackagingLevelIndex { get; set; }

        /// <summary>Master UDI-DI only. 1-19 chars of 0-9, A-Z, '.', '-'.</summary>
        public string DeviceGroupCode { get; set; }

        /// <summary>
        /// AIC and AIM only. Opaque national code. 1-18 chars of 0-9, A-Z, '.', '-'. Bounds are
        /// provisional -- IFA does not publish a format spec for this code.
        /// </summary>
        public string NationalCode { get; set; }
    }

    public sealed class BuildUdiPiInput
    {
        public string Lot { get; set; }

        /// <summary>"YYYY-MM-DD", or "YYYY-MM" for an unspecified day.</summary>
        public string ExpiryDate { get; set; }

        /// <summary>"YYYY-MM-DD" only.</summary>
        public string ManufacturingDate { get; set; }

        public string SerialNumber { get; set; }
        public int? Quantity { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }
        public IReadOnlyList<string> AdditionalGtins { get; set; }
    }

    public sealed class BuildUdiInput
    {
        public BuildUdiDiInput UdiDi { get; set; }
        public BuildUdiPiInput UdiPi { get; set; }
    }
}
