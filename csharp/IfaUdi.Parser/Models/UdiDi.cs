namespace IfaUdi.Parser.Models;

public sealed record UdiDi
{
    public required string Raw { get; init; }
    public required UdiScheme Scheme { get; init; }
    public required string PraCode { get; init; }
    public required string CheckDigits { get; init; }

    /// <summary>PPN only.</summary>
    public string? Pzn { get; init; }

    /// <summary>HPC and Master UDI-DI only.</summary>
    public string? Cin { get; init; }

    /// <summary>HPC only.</summary>
    public string? ItemReference { get; init; }

    /// <summary>HPC only. 0-8.</summary>
    public int? PackagingLevelIndex { get; init; }

    /// <summary>Master UDI-DI only.</summary>
    public string? DeviceGroupCode { get; init; }
}
