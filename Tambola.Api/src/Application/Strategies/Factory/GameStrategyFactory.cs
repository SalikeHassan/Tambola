using Tambola.Api.src.Application.Common; 
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies.Factory;

public class GameStrategyFactory : IGameStrategyFactory
{
    private readonly IEnumerable<IGameStrategy> strategies;
    public GameStrategyFactory(IEnumerable<IGameStrategy> strategies)
    {
        this.strategies = strategies;
    }

    public IGameStrategy Create(GameType gameType)
    {
        var strategy = strategies.FirstOrDefault(s => s.Key == gameType);
        return strategy ?? throw new ArgumentException(Constants.InvalidGameTypeErrMsg);
    }
}
