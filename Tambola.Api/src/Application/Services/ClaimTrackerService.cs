using System.Collections.Concurrent;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Services;

public class ClaimTrackerService : IClaimTrackerService
{
    private static readonly ConcurrentDictionary<GameType, List<string>> claims = new();
    private static readonly object lockObj = new();

    public bool HasPlayerAlreadyClaimed(GameType gameType, string playerId)
    {
        return claims.TryGetValue(gameType, out var winners) && winners.Contains(playerId);
    }

    public bool RegisterClaim(GameType gameType, string playerId)
    {
        lock (lockObj)
        {
            if (claims.TryGetValue(gameType, out var winners))
            {
                if (winners.Contains(playerId))
                {
                    return false;
                }
                winners.Add(playerId);
            }
            else
            {
                claims.TryAdd(gameType, new List<string> { playerId });
            }
        }
        return true;
    }
}
