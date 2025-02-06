using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Commands;

public class ClaimCommand : ICommand<Result<ClaimResponse>>  
{
    public required Guid PlayerId { get; set; }
    public required int?[][] TicketNumbers { get; set; }
    public required List<int> AnnouncedNumbers { get; set; }
    public required GameType ClaimType { get; set; }
}