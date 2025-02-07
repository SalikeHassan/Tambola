using Tambola.Api.src.Application.Common;

namespace Tambola.Api.src.Application.Commands;

public record ClaimResponse(bool IsSuccess, string Message)
{
    public static Result<ClaimResponse> Winner(string msg) => 
        Result<ClaimResponse>.Success(new(true, msg), StatusCodes.Status200OK);
    
    public static Result<ClaimResponse> Lost(string msg) => 
        Result<ClaimResponse>.Fail(msg, StatusCodes.Status400BadRequest);
}