using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Services;

public interface IClaimTrackerService
{
    bool HasPlayerAlreadyClaimed(GameType gameType, string playerId);

    bool RegisterClaim(GameType gameType, string playerId);
}
