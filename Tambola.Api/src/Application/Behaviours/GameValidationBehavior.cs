using MediatR;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Behaviors;

public class GameValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ClaimCommand
    where TResponse : SubmitClaimResult
{
    private readonly IClaimValidationService claimValidationService;

    public GameValidationBehavior(IClaimValidationService claimValidationService)
    {
        this.claimValidationService = claimValidationService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Ticket ticket = Ticket.Create(request.TicketNumbers);
        bool isWinning = claimValidationService.IsClaimValid(ticket, request.AnnouncedNumbers, request.ClaimType);

        if (!isWinning)
        {
            return (TResponse)(object)new SubmitClaimResult(false, "Rejected: Claim is not valid.");
        }

        return await next();
    }
}
