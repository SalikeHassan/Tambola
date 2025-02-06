using MediatR;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Behaviors;

public class GameValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ClaimCommand
    where TResponse : Result<ClaimResponse>
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
            return (TResponse)Result<ClaimResponse>.Fail("Rejected",StatusCodes.Status400BadRequest);
        }

        return await next();
    }
}
