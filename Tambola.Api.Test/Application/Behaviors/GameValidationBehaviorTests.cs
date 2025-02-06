using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shouldly;
using Xunit;
using Tambola.Api.src.Application.Behaviors;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;
using Tambola.Api.src.Application.Common;

namespace Tambola.Api.Test.Application.Behaviors;

public class GameValidationBehaviorTests
{
    private readonly IClaimValidationService claimValidationService;
    private readonly GameValidationBehavior<ClaimCommand, Result<ClaimResponse>> behavior;

    public GameValidationBehaviorTests()
    {
        this.claimValidationService = A.Fake<IClaimValidationService>();
        this.behavior = new GameValidationBehavior<ClaimCommand, Result<ClaimResponse>>(claimValidationService);
    }

    [Fact]
    public async Task Handle_ClaimIsValid_ShouldCallNext()
    {
        // Arrange
        var request = new ClaimCommand
        {
            PlayerId = Guid.NewGuid(),
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
        A.CallTo(() => claimValidationService.IsClaimValid(A<Ticket>._, request.AnnouncedNumbers, request.ClaimType)).Returns(true);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        A.CallTo(() => next()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_ClaimIsInvalid_ShouldReturnFailure()
    {
        // Arrange
        var request = new ClaimCommand
        {
            PlayerId = Guid.NewGuid(),
            TicketNumbers = new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            },
            AnnouncedNumbers = new List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };
        A.CallTo(() => claimValidationService.IsClaimValid(A<Ticket>._, request.AnnouncedNumbers, request.ClaimType)).Returns(false);

        // Act
        var result = await behavior.Handle(request, null, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe(Constants.RejectedErrMsg);
    }
}