using System.Collections.Generic;
using FakeItEasy;
using Shouldly;
using MediatR;
using Tambola.Api.src.Application.Behaviors;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Domain;
using Tambola.Api.src.Application.Common;
using Xunit;

namespace Tambola.Api.Test.Behaviors;

public class DuplicateClaimBehaviorTests
{
    private readonly IClaimTrackerService claimTrackerService;
    private readonly DuplicateClaimBehavior<ClaimCommand, Result<ClaimResponse>> behavior;

    public DuplicateClaimBehaviorTests()
    {
        claimTrackerService = A.Fake<IClaimTrackerService>();
        behavior = new DuplicateClaimBehavior<ClaimCommand, Result<ClaimResponse>>(claimTrackerService);
    }

    [Fact]
    public async Task Handle_PlayerHasNotClaimed_ShouldCallNext()
    {
        // Arrange
        var request = new ClaimCommand
        {
            PlayerId = "Player1",
            ClaimType = GameType.TopLine,
            TicketNumbers = new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            },
            AnnouncedNumbers = new List<int> { 90, 4, 46, 63, 89, 16, 76, 48 }
        };

        var next = A.Fake<RequestHandlerDelegate<Result<ClaimResponse>>>();
        A.CallTo(() => claimTrackerService.HasPlayerAlreadyClaimed(request.ClaimType, request.PlayerId)).Returns(false);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        A.CallTo(() => next()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_PlayerHasAlreadyClaimed_ShouldReturnFailure()
    {
        // Arrange
        var request = new ClaimCommand
        {
            PlayerId = "Player1",
            ClaimType = GameType.TopLine,
            TicketNumbers = new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            },
            AnnouncedNumbers = new List<int> { 90, 4, 46, 63, 89, 16, 76, 48 }
        };

        A.CallTo(() => claimTrackerService.HasPlayerAlreadyClaimed(request.ClaimType, request.PlayerId)).Returns(true);

        var next = A.Fake<RequestHandlerDelegate<Result<ClaimResponse>>>();

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(Constants.DuplicateClaimErrMsg);
    }
}
