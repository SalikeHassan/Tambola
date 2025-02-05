using MediatR;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Behaviors;

public class DuplicateClaimBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ClaimCommand
    where TResponse : SubmitClaimResult
{
    private readonly IClaimTrackerService claimTrackerService;

    public DuplicateClaimBehavior(IClaimTrackerService claimTrackerService)
    {
        this.claimTrackerService = claimTrackerService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (this.claimTrackerService.HasPlayerAlreadyClaimed(request.ClaimType, request.PlayerId))
        {
            return (TResponse)(object)new SubmitClaimResult(false, "Rejected: You have already claimed this game.");
        }

        return await next();
    }
}
