namespace IfaUdi.Parser.Models
{
    public sealed class ParsedUdi
    {
        /// <summary>The original barcode string as passed to ParseUdi.</summary>
        public string Raw { get; set; }
        public UdiDi UdiDi { get; set; }
        public UdiPi UdiPi { get; set; }
    }
}
