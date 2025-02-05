using Tambola.Api.src.Application.Strategies.Factory;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Validators;

public class ClaimValidator : IClaimValidator
{
    private readonly IGameStrategyFactory strategyFactory;

    public ClaimValidator(IGameStrategyFactory strategyFactory)
    {
        this.strategyFactory = strategyFactory;
    }
    public bool ValidateClaim(Ticket ticket, List<int> announcedNumbers, GameType gameType)
    {
        var strategy = strategyFactory.Create(gameType);

        if (strategy == null)
        {
            throw new ArgumentException("Invalid game type.");
        }

        bool isWinningNow = strategy.IsWinningCondition(ticket, announcedNumbers);

        var numbersBeforeLast = announcedNumbers.Take(announcedNumbers.Count - 1).ToList();
        bool wasWinningBefore = strategy.IsWinningCondition(ticket, numbersBeforeLast);

        return isWinningNow && !wasWinningBefore;
    }
}
