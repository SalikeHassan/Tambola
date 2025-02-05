using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies;

public class MiddleLineStrategy : IGameStrategy
{
        public GameType Key => GameType.MiddleLine;

    public bool IsWinningCondition(Ticket ticket, List<int> announcedNumbers)
    {
        var middleLine = ticket.GetRow(1);
        return middleLine.All(num => announcedNumbers.Contains(num));
    }
}
