using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies;

public class FullHouseStrategy : IGameStrategy
{
        public GameType Key => GameType.FullHouse;

    public bool IsWinningCondition(Ticket ticket, List<int> announcedNumbers)
    {
        var allNumbers = ticket.GetRow(0).Concat(ticket.GetRow(1)).Concat(ticket.GetRow(2)).ToList();
        return allNumbers.All(num => announcedNumbers.Contains(num));
    }
}
