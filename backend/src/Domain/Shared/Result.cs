using TTRPG.Toolkit.Domain.Enums;

namespace TTRPG.Toolkit.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(string code, string description, ErrorType type) =>
        new(false, new(code, description, type));
}