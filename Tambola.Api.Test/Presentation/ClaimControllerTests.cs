using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Http; // Required for StatusCodes
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using Tambola.Api.src.Application.Commands;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Presentation.Controllers;
using Tambola.Api.src.Domain;

namespace Tambola.Api.Test.Presentation;

public class ClaimControllerTests
{
    private readonly IMediator mediator;
    private readonly ClaimController controller;

    public ClaimControllerTests()
    {
        mediator = A.Fake<IMediator>();
        controller = new ClaimController(mediator);
    }

    [Fact]
    public async Task ValidateClaim_ShouldReturnOk_WhenClaimIsSuccessful()
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
            AnnouncedNumbers = new System.Collections.Generic.List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };

        var successResult = Result<ClaimResponse>.Success(new ClaimResponse(true, "Claim accepted"), StatusCodes.Status200OK);

        A.CallTo(() => mediator.Send(command, A<CancellationToken>._))
            .Returns(successResult);

        // Act
        var response = await controller.ValidateClaim(command);

        // Assert
        response.ShouldBeOfType<ObjectResult>(); // Since it contains a value
        var okResult = response as ObjectResult;
        okResult?.StatusCode.ShouldBe(StatusCodes.Status200OK);
        okResult?.Value.ShouldBe(successResult);
    }

    [Fact]
    public async Task ValidateClaim_ShouldReturnBadRequest_WhenClaimFails()
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
            AnnouncedNumbers = new System.Collections.Generic.List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };

        var failureResult = Result<ClaimResponse>.Fail("Claim rejected", StatusCodes.Status400BadRequest);

        A.CallTo(() => mediator.Send(command, A<CancellationToken>._))
            .Returns(failureResult);

        // Act
        var response = await controller.ValidateClaim(command);

        // Assert
        response.ShouldBeOfType<ObjectResult>(); // Since it contains a value
        var badRequestResult = response as ObjectResult;
        badRequestResult?.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        badRequestResult?.Value.ShouldBe(failureResult);
    }

    [Fact]
    public async Task ValidateClaim_ShouldReturnBadRequest_WhenCommandIsNull()
    {
        // Act
        var response = await controller.ValidateClaim(null);

        // Assert
        response.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = response as BadRequestObjectResult;
        badRequestResult?.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        badRequestResult?.Value.ShouldBe(Result<string>.Fail("Invalid request. ClaimCommand cannot be null.", StatusCodes.Status400BadRequest));
    }

    [Fact]
    public async Task ValidateClaim_ShouldReturnInternalServerError_OnException()
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
            AnnouncedNumbers = new System.Collections.Generic.List<int> { 4, 16, 48, 63, 76 },
            ClaimType = GameType.TopLine
        };

        A.CallTo(() => mediator.Send(command, A<CancellationToken>._))
            .Throws(new Exception("Unexpected error"));

        // Act
        var response = await controller.ValidateClaim(command);

        // Assert
        response.ShouldBeOfType<ObjectResult>(); // Since it contains a value
        var errorResult = response as ObjectResult;
        errorResult?.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        errorResult?.Value.ShouldBe(Result<string>.Fail("An unexpected error occurred: Unexpected error", StatusCodes.Status500InternalServerError));
    }
}