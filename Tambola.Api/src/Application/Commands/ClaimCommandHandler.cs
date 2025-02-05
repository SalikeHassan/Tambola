using System.Collections.Concurrent;
using Tambola.Api.src.Application.Validators;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Commands;

public class ClaimCommandHandler : ICommandHandler<ClaimCommand, SubmitClaimResult>
{
    private static readonly ConcurrentDictionary<GameType, List<string>> AcceptedClaims = new();
    private static readonly object lockObj = new();

    private readonly IClaimValidator _claimValidator;

    public ClaimCommandHandler(IClaimValidator claimValidator)
    {
        _claimValidator = claimValidator;
    }

    public async Task<SubmitClaimResult> Handle(ClaimCommand command, CancellationToken cancellationToken)
    {
        // Validate the ticket format
        Ticket ticket;
        try
        {
            ticket = Ticket.Create(command.TicketNumbers);
        }
        catch (ArgumentException ex)
        {
            return new SubmitClaimResult(false, $"Invalid ticket: {ex.Message}");
        }

        // Validate the claim using the game-specific strategy
        bool isWinning = _claimValidator.ValidateClaim(ticket, command.AnnouncedNumbers, command.ClaimType);

        if (!isWinning)
        {
            return new SubmitClaimResult(false, "Rejected: Claim is not valid.");
        }

        lock (lockObj)
        {
            if (AcceptedClaims.TryGetValue(command.ClaimType, out var winners))
            {
                if (winners.Contains(command.PlayerId))
                {
                    return new SubmitClaimResult(false, "Rejected: You have already claimed this game.");
                }

                winners.Add(command.PlayerId);
                return new SubmitClaimResult(true, "Accepted: You are a winner!");
            }

            AcceptedClaims.TryAdd(command.ClaimType, new List<string> { command.PlayerId });
            return new SubmitClaimResult(true, "Accepted: You are a winner!");
        }
    }
}