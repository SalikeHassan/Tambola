using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Strategies.Factory;

public interface IGameStrategyFactory
{
    IGameStrategy Create(GameType gameType);
}
