using System.Collections.Generic;

namespace IfaUdi.Parser.Models
{
    public sealed class UdiPi
    {
        /// <summary>Exact source substring covering the UDI-PI fields, empty string if there are none.</summary>
        public string Raw { get; set; }

        public string Lot { get; set; }

        /// <summary>Formatted as YYYY-MM-DD, or YYYY-MM when the source day is "00" (unspecified).</summary>
        public string ExpiryDate { get; set; }

        /// <summary>Formatted as YYYY-MM-DD.</summary>
        public string ManufacturingDate { get; set; }

        public string SerialNumber { get; set; }
        public int? Quantity { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }
        public IReadOnlyList<string> AdditionalGtins { get; set; }
    }
}
