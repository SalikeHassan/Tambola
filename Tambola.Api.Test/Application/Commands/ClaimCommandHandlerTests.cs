using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Shouldly;
using Xunit;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;

namespace Tambola.Api.Test.Application.Commands;

public class ClaimCommandHandlerTests
{
    private readonly IClaimTrackerService claimTrackerService;
    private readonly ClaimCommandHandler handler;

    public ClaimCommandHandlerTests()
    {
        claimTrackerService = A.Fake<IClaimTrackerService>();
        handler = new ClaimCommandHandler(claimTrackerService);
    }

    [Fact]
    public async Task ClaimCommandHandlerHandler_ShouldReturnSuccess_WhenClaimIsRegistered()
    {
        // Arrange
        var command = new ClaimCommand
        {
            PlayerId = "Player1",
            TicketNumbers = new int?[][]
            {
                    new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                    new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                    new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            },
            AnnouncedNumbers = new List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };

        A.CallTo(() => claimTrackerService.RegisterClaim(command.ClaimType, command.PlayerId))
            .Returns(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Message.ShouldBe("Accepted");
    }

    [Fact]
    public async Task ClaimCommandHandlerHandler_ShouldReturnFailure_WhenClaimIsAlreadyRegistered()
    {
        // Arrange
        var command = new ClaimCommand
        {
            PlayerId = "Player1",
            TicketNumbers = new int?[][]
            {
                    new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                    new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                    new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            },
            AnnouncedNumbers = new List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };

        A.CallTo(() => claimTrackerService.RegisterClaim(command.ClaimType, command.PlayerId))
            .Returns(false);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Rejected");
    }
}