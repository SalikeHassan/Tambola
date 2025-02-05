using Microsoft.AspNetCore.Mvc;
using MediatR;
using Tambola.Api.src.Application.Commands;

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

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateClaim([FromBody] ClaimCommand command)
    {
        if (command == null)
        {
            return BadRequest("Invalid request. ClaimCommand cannot be null.");
        }

        try
        {
            // Send the command to MediatR
            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });
            }
        }
        catch (Exception ex)
        {
            // Handle unexpected errors
            return StatusCode(500, new
            {
                success = false,
                message = $"An unexpected error occurred: {ex.Message}"
            });
        }
    }
}
