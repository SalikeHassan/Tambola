using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Services;

public interface IClaimValidationService
{
    bool IsClaimValid(Ticket ticket, List<int> announcedNumbers, GameType gameType);
}
