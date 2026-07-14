using System.Text.RegularExpressions;

namespace IfaUdi.Parser
{
    /// <summary>
    /// Shared validation rules used by both the parser and the builder, so parse-side and
    /// build-side rules cannot drift apart. See documentation/IFA_UDI_Parser_Analysis.md sections 2-3.
    /// </summary>
    internal static class Validation
    {
        /// <summary>ASCII characters technically excluded from LOT/SN.</summary>
        public static readonly Regex ForbiddenLotSnChars = new Regex(@"[\x00-\x1F\x7F-\xFF#$@\[\\\]\^`{|}~]", RegexOptions.Compiled);

        public static readonly Regex ItemReferenceCharset = new Regex(@"^[0-9A-Z.\-]+$", RegexOptions.Compiled);

        public static readonly Regex AlphanumericUpperCharset = new Regex(@"^[0-9A-Z]+$", RegexOptions.Compiled);
    }
}
