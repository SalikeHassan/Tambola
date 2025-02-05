
using Tambola.Api.src.Application.Validators;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Services;

public class ClaimValidationService : IClaimValidationService
{
    private readonly IClaimValidator validator;

    public ClaimValidationService(IClaimValidator validator)
    {
        this.validator = validator;
    }

    public bool IsClaimValid(Ticket ticket, List<int> announcedNumbers, GameType gameType)
    {
         return validator.ValidateClaim(ticket, announcedNumbers, gameType);
    }
}
