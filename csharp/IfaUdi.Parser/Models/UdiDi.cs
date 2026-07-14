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
    }
}
