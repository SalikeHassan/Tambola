using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Domain;

namespace Tambola.Api.src.Application.Commands;

public class ClaimCommand : ICommand<Result<ClaimResponse>>  
{
    public string PlayerId { get; set; }
    public int?[][] TicketNumbers { get; set; }
    public List<int> AnnouncedNumbers { get; set; }
    public GameType ClaimType { get; set; }
}