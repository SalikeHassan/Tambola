namespace Tambola.Api.src.Domain;

public class Ticket
{
    public int?[][] Numbers { get; private set; }

    private Ticket(int?[][] numbers)
    {
        if (numbers.Length != 3 || numbers.Any(row => row.Length != 9))
        {
            throw new ArgumentException("Invalid ticket format. Must be a 3x9 grid.");
        }

        Numbers = numbers;
    }

    public static Ticket Create(int?[][] numbers) => new Ticket(numbers);

    public List<int> GetRow(int rowIndex) =>
        Numbers[rowIndex].Where(num => num.HasValue).Select(num => num!.Value).ToList();
}