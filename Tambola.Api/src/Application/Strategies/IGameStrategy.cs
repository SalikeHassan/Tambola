using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies;

public interface IGameStrategy
{
    GameType Key { get; }
    bool IsWinningCondition(Ticket ticket, List<int> announcedNumbers);
}
