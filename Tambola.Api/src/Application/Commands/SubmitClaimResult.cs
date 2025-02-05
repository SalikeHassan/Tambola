namespace Tambola.Api.src.Application.Commands;

public class SubmitClaimResult
{
    public bool IsSuccess { get; }
    public string Message { get; }

    public SubmitClaimResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static SubmitClaimResult Winner() => new(true, "🏆 Congratulations! You Won!");
    public static SubmitClaimResult Lost(string reason) => new(false, $"❌ You Lost! {reason}");
}

