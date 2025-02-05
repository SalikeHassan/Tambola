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

    public async Task<Result<ClaimResponse>> Handle(ClaimCommand command, CancellationToken cancellationToken)
    {
        if (!claimTrackerService.RegisterClaim(command.ClaimType, command.PlayerId))
        {
            return ClaimResponse.Lost("Rejected");
        }

        return ClaimResponse.Winner("Accepted");
    }
}
