using Shouldly;
using Tambola.Api.src.Application.Strategies;
using Tambola.Api.src.Domain;
using Xunit;

namespace Tambola.Api.Test.Application.Strategies.Tests;

public class FullHouseStrategyTests
{
    private readonly FullHouseStrategy strategy;

    public FullHouseStrategyTests()
    {
        strategy = new FullHouseStrategy();
    }

    [Fact]
    public void IsWinningCondition_AllNumbersAnnounced_ShouldReturnTrue()
    {
        // Arrange
        var ticket = Ticket.Create(new int?[][]
        {
            new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
            new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
            new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
        });
        var announcedNumbers = new List<int> { 4, 16, 48, 63, 76, 7, 23, 38, 52, 80, 9, 25, 56, 64, 83 };

        // Act
        var result = strategy.IsWinningCondition(ticket, announcedNumbers);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void IsWinningCondition_NotAllNumbersAnnounced_ShouldReturnFalse()
    {
        // Arrange
        var ticket = Ticket.Create(new int?[][]
        {
            new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
            new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
            new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
        });
        var announcedNumbers = new List<int> { 4, 16, 48, 63, 76, 7, 23, 38, 52, 80, 9, 25, 56, 64 };

        // Act
        var result = strategy.IsWinningCondition(ticket, announcedNumbers);

        // Assert
        result.ShouldBeFalse();
    }
}
