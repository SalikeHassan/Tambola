using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Validators;

public interface IClaimValidator
{
    bool ValidateClaim(Ticket ticket, List<int> announcedNumbers, GameType gameType);
}

