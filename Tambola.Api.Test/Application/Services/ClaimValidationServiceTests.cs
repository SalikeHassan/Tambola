using FakeItEasy;
using Shouldly;
using Xunit;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Application.Validators;
using Tambola.Api.src.Domain;

namespace Tambola.Api.Test.Application.Services;

public class ClaimValidationServiceTests
{
    private readonly IClaimValidator validator;
    private readonly ClaimValidationService claimValidationService;

    public ClaimValidationServiceTests()
    {
        validator = A.Fake<IClaimValidator>();
        claimValidationService = new ClaimValidationService(validator);
    }

    [Fact]
    public void IsClaimValid_ClaimIsValid_ShouldReturnTrue()
    {
        // Arrange
        var ticket = Ticket.Create(new int?[][]
        {
            new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
            new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
            new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
        });
        var announcedNumbers = new List<int> { 4, 16, 48, 63, 76 };
        var gameType = GameType.TopLine;
        A.CallTo(() => validator.ValidateClaim(ticket, announcedNumbers, gameType)).Returns(true);

        // Act
        var result = claimValidationService.IsClaimValid(ticket, announcedNumbers, gameType);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void IsClaimValid_ClaimIsInvalid_ShouldReturnFalse()
    {
        // Arrange
        var ticket = Ticket.Create(new int?[][]
        {
            new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
            new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
            new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
        });
        var announcedNumbers = new List<int> { 4, 16, 48, 63 };
        var gameType = GameType.TopLine;
        A.CallTo(() => validator.ValidateClaim(ticket, announcedNumbers, gameType)).Returns(false);

        // Act
        var result = claimValidationService.IsClaimValid(ticket, announcedNumbers, gameType);

        // Assert
        result.ShouldBeFalse();
    }
}
