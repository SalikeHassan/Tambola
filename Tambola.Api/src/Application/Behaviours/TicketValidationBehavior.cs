using MediatR;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Behaviors;

public class TicketValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ClaimCommand
    where TResponse : SubmitClaimResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            Ticket.Create(request.TicketNumbers);
        }
        catch (ArgumentException ex)
        {
            return (TResponse)(object)new SubmitClaimResult(false, $"Invalid ticket: {ex.Message}");
        }

        return await next();
    }
}
