using TTRPG.Toolkit.Domain.Enums;

namespace TTRPG.Toolkit.Domain.Shared;

public record Error(
    string Code,
    string Description,
    ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}