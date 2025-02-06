using Tambola.Api.src.Application.Common;

namespace Tambola.Api.src.Domain;

public class Ticket
{
    private int?[][] Numbers { get; set; }

    private Ticket(int?[][] numbers)
    {
        if (numbers.Length != 3 || numbers.Any(row => row.Length != 9))
        {
            throw new ArgumentException(Constants.InvalidTicketErrMsg);
        }

        Numbers = numbers;
    }

    public static Ticket Create(int?[][] numbers) => new Ticket(numbers);

    public List<int> GetRow(int rowIndex) =>
        Numbers[rowIndex].Where(num => num.HasValue).Select(num => num!.Value).ToList();
}