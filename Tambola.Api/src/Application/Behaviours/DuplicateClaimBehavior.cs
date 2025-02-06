using MediatR;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;

namespace Tambola.Api.src.Application.Behaviors;

public class DuplicateClaimBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ClaimCommand
    where TResponse : Result<ClaimResponse>
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
            return (TResponse)Result<ClaimResponse>.Fail("You have already claimed this game.",StatusCodes.Status400BadRequest);
        }

        return await next();
    }
}
