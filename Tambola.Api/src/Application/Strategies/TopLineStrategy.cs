using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies;

public class TopLineStrategy : IGameStrategy
{
    public GameType Key => GameType.TopLine;
    public bool IsWinningCondition(Ticket ticket, List<int> announcedNumbers)
    {
        var topLine = ticket.GetRow(0);

        return topLine.All(num=>announcedNumbers.Contains(num));
    }
}
