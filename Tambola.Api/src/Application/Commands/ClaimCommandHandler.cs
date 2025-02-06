using MediatR;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;

namespace Tambola.Api.src.Application.Commands;

public class ClaimCommandHandler : IRequestHandler<ClaimCommand, Result<ClaimResponse>>
{
    private readonly IClaimTrackerService claimTrackerService;

    public ClaimCommandHandler(IClaimTrackerService claimTrackerService)
    {
        this.claimTrackerService = claimTrackerService;
    }

    public Task<Result<ClaimResponse>> Handle(ClaimCommand command, CancellationToken cancellationToken)
    {
        if (!claimTrackerService.RegisterClaim(command.ClaimType, command.PlayerId))
        {
            return Task.FromResult(ClaimResponse.Lost(Constants.RejectedErrMsg));
        }

        return Task.FromResult(ClaimResponse.Winner(Constants.AcceptedMsg));
    }
}
