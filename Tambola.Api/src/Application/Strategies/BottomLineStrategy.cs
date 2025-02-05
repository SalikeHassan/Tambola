using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies;

public class BottomLineStrategy : IGameStrategy
{
    public GameType Key => GameType.BottomLine;

    public bool IsWinningCondition(Ticket ticket, List<int> announcedNumbers)
    {
        var bottomLine = ticket.GetRow(2);
        return bottomLine.All(num => announcedNumbers.Contains(num));
    }
}
