namespace IfaUdi.Parser.Models;

public sealed record ParsedUdi
{
    public required UdiDi UdiDi { get; init; }
    public required UdiPi UdiPi { get; init; }
}
