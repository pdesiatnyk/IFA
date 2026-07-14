namespace IfaUdi.Parser.Models;

public sealed record UdiPi
{
    public string? Lot { get; init; }

    /// <summary>Formatted as YYYY-MM-DD, or YYYY-MM when the source day is "00" (unspecified).</summary>
    public string? ExpiryDate { get; init; }

    /// <summary>Formatted as YYYY-MM-DD.</summary>
    public string? ManufacturingDate { get; init; }

    public string? SerialNumber { get; init; }
    public int? Quantity { get; init; }
    public string? Price { get; init; }
    public string? Url { get; init; }
    public IReadOnlyList<string>? AdditionalGtins { get; init; }
}
