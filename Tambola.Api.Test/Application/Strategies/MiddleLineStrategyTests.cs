using Shouldly;
using Xunit;
using Tambola.Api.src.Application.Strategies;
using Tambola.Api.src.Domain;

namespace Tambola.Api.Test.Application.Strategies;

public class MiddleLineStrategyTests
{
    private readonly MiddleLineStrategy strategy;

    public MiddleLineStrategyTests()
    {
        strategy = new MiddleLineStrategy();
    }

    [Fact]
    public void IsWinningCondition_AllNumbersInMiddleLineAnnounced_ShouldReturnTrue()
    {
        // Arrange
        var ticket = Ticket.Create(new int?[][]
        {
            new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
            new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
            new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
        });
        var announcedNumbers = new List<int> { 7, 23, 38, 52, 80 };

        // Act
        var result = strategy.IsWinningCondition(ticket, announcedNumbers);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void IsWinningCondition_NotAllNumbersInMiddleLineAnnounced_ShouldReturnFalse()
    {
        // Arrange
        var ticket = Ticket.Create(new int?[][]
        {
            new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
            new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
            new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
        });
        var announcedNumbers = new List<int> { 7, 23, 38, 52 };

        // Act
        var result = strategy.IsWinningCondition(ticket, announcedNumbers);

        // Assert
        result.ShouldBeFalse();
    }
}