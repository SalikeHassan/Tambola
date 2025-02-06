using Tambola.Api.src.Application.Behaviors;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;
using FakeItEasy;
using MediatR;
using Shouldly;
using Xunit;

namespace Tambola.Api.Test.Application.Behaviors;

public class TicketValidationBehaviorTests
{
    private readonly TicketValidationBehavior<ClaimCommand, Result<ClaimResponse>> behavior;

    public TicketValidationBehaviorTests()
    {
        behavior = new TicketValidationBehavior<ClaimCommand, Result<ClaimResponse>>();
    }

    [Fact]
    public async Task Handle_TicketIsValid_ShouldCallNext()
    {
        // Arrange
        var request = new ClaimCommand
        {
            PlayerId = "Player123",
            TicketNumbers = new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            },
            AnnouncedNumbers = new List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };
        var next = A.Fake<RequestHandlerDelegate<Result<ClaimResponse>>>();

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        A.CallTo(() => next()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_TicketIsInvalid_ShouldReturnFailure()
    {
        // Arrange
        var request = new ClaimCommand
        {
            PlayerId = "Player123",
            TicketNumbers = new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 } // Missing third row
            },
            AnnouncedNumbers = new List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };

        // Act
        var result = await behavior.Handle(request, null, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldStartWith(Constants.InvalidTicketErrMsg);
    }
}