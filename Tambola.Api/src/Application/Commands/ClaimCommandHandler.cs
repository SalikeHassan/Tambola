using MediatR;
using Tambola.Api.src.Application.Services;

namespace Tambola.Api.src.Application.Commands;

public class ClaimCommandHandler : IRequestHandler<ClaimCommand, SubmitClaimResult>
{
    private readonly IClaimTrackerService claimTrackerService;

    public ClaimCommandHandler(IClaimTrackerService claimTrackerService)
    {
        this.claimTrackerService = claimTrackerService;
    }

    public async Task<SubmitClaimResult> Handle(ClaimCommand command, CancellationToken cancellationToken)
    {
        if (!claimTrackerService.RegisterClaim(command.ClaimType, command.PlayerId))
        {
            return new SubmitClaimResult(false, "Rejected: Duplicate claim.");
        }

        return new SubmitClaimResult(true, "Accepted: You are a winner!");
    }
}
