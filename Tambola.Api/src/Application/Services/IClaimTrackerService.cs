using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Services;

public interface IClaimTrackerService
{
    bool HasPlayerAlreadyClaimed(GameType gameType, Guid playerId);

    bool RegisterClaim(GameType gameType, Guid playerId);
}
