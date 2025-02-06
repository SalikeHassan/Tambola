namespace Tambola.Api.src.Application.Common;

public record Result<T>(T? Value, string? ErrorMessage, bool IsSuccess, int StatusCode)
{
    public static Result<T> Success(T value, int statusCode) => new(value, null, true, statusCode);
    public static Result<T> Fail(string message, int statusCode) => new(default, message, false, statusCode);
}
