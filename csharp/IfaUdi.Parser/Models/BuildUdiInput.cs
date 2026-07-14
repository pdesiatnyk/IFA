namespace IfaUdi.Parser.Models;

public sealed record BuildUdiDiInput
{
    public required UdiScheme Scheme { get; init; }

    /// <summary>PPN only. 7-digit base; the 8th (mod-11) PZN check digit is always computed.</summary>
    public string? PznBase { get; init; }

    /// <summary>HPC and Master UDI-DI only. Exactly 5 alphanumeric-uppercase characters.</summary>
    public string? Cin { get; init; }

    /// <summary>HPC only. 1-18 chars of 0-9, A-Z, '.', '-'.</summary>
    public string? ItemReference { get; init; }

    /// <summary>HPC only. 0-8 (9 is reserved for variable quantities, invalid for UDI).</summary>
    public int? PackagingLevelIndex { get; init; }

    /// <summary>Master UDI-DI only. 1-19 chars of 0-9, A-Z, '.', '-'.</summary>
    public string? DeviceGroupCode { get; init; }
}

public sealed record BuildUdiPiInput
{
    public string? Lot { get; init; }

    /// <summary>"YYYY-MM-DD", or "YYYY-MM" for an unspecified day.</summary>
    public string? ExpiryDate { get; init; }

    /// <summary>"YYYY-MM-DD" only.</summary>
    public string? ManufacturingDate { get; init; }

    public string? SerialNumber { get; init; }
    public int? Quantity { get; init; }
    public string? Price { get; init; }
    public string? Url { get; init; }
    public IReadOnlyList<string>? AdditionalGtins { get; init; }
}

public sealed record BuildUdiInput
{
    public required BuildUdiDiInput UdiDi { get; init; }
    public BuildUdiPiInput? UdiPi { get; init; }
}
