using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies;

public class EarlyFiveStrategy : IGameStrategy
{
    public GameType Key => GameType.EarlyFive;

    public bool IsWinningCondition(Ticket ticket, List<int> announcedNumbers)
    {
        var allNumbers = ticket.GetRow(0).Concat(ticket.GetRow(1)).Concat(ticket.GetRow(2)).ToList();
        return allNumbers.Count(num => announcedNumbers.Contains(num)) >= 5;
    }
}