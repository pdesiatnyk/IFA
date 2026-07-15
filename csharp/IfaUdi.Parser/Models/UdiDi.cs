namespace IfaUdi.Parser.Models
{
    public sealed class UdiDi
    {
        public string Raw { get; set; }
        public UdiScheme Scheme { get; set; }
        public string PraCode { get; set; }
        public string CheckDigits { get; set; }

        /// <summary>PPN only.</summary>
        public string Pzn { get; set; }

        /// <summary>HPC and Master UDI-DI only.</summary>
        public string Cin { get; set; }

        /// <summary>HPC only.</summary>
        public string ItemReference { get; set; }

        /// <summary>HPC only. 0-8.</summary>
        public int? PackagingLevelIndex { get; set; }

        /// <summary>Master UDI-DI only.</summary>
        public string DeviceGroupCode { get; set; }

        /// <summary>
        /// AIC and AIM only. Opaque national code (Italy AIC / Portugal AIM) -- IFA does not
        /// publish a length/charset/check-digit spec for this code, so only overall bounds
        /// (1-18 chars, 0-9A-Z.-) plus the outer Mod-97 checksum are validated.
        /// </summary>
        public string NationalCode { get; set; }
    }
}
