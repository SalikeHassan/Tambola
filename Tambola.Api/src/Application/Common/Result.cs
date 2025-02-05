namespace Tambola.Api.src.Application.Common;

public record Result<T>(T? Value, string? ErrorMessage, bool IsSuccess)
{
    public static Result<T> Success(T value) => new(value, null, true);
    public static Result<T> Fail(string message) => new(default, message, false);
}
