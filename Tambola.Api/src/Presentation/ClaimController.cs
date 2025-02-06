using Microsoft.AspNetCore.Mvc;
using MediatR;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;

namespace Tambola.Api.src.Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/claim")]
[ApiVersion("1.0")]
public class ClaimController : ControllerBase
{
    private readonly IMediator mediator;

    public ClaimController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> ValidateClaim([FromBody] ClaimCommand command)
    {
        if (command == null)
        {
            return BadRequest(Result<string>.Fail("Invalid request. ClaimCommand cannot be null.", StatusCodes.Status400BadRequest));
        }

        try
        {
            var result = await mediator.Send(command);

            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                Result<string>.Fail($"An unexpected error occurred: {ex.Message}", StatusCodes.Status500InternalServerError));
        }
    }
}