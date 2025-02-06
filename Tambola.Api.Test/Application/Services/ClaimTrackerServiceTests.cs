using System.Collections.Generic;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using Tambola.Api.src.Application.Services;
using Tambola.Api.src.Domain;

namespace Tambola.Api.Test.Application.Services;

[TestCaseOrderer("Tambola.Api.Test.Application.Services.AlphabeticalOrderer", "Tambola.Api.Test")]
public class ClaimTrackerServiceTests
{
    private readonly IClaimTrackerService claimTrackerService;
    private Guid playerId;

    public ClaimTrackerServiceTests()
    {
        claimTrackerService = new ClaimTrackerService();
        playerId = Guid.NewGuid();
    }

    [Fact, TestPriority(1)]
    public void RegisterClaim_ShouldReturnTrue_WhenClaimIsRegisteredSuccessfully()
    {
        // Act
        var result = claimTrackerService.RegisterClaim(GameType.TopLine, playerId);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact, TestPriority(2)]
    public void RegisterClaim_ShouldReturnFalse_WhenPlayerHasAlreadyClaimed()
    {
        // Arrange
        claimTrackerService.RegisterClaim(GameType.TopLine, playerId);

        // Act
        var result = claimTrackerService.RegisterClaim(GameType.TopLine, playerId);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact, TestPriority(3)]
    public void HasPlayerAlreadyClaimed_ShouldReturnTrue_WhenPlayerHasAlreadyClaimed()
    {
        // Arrange
        claimTrackerService.RegisterClaim(GameType.TopLine, playerId);

        // Act
        var result = claimTrackerService.HasPlayerAlreadyClaimed(GameType.TopLine, playerId);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact, TestPriority(4)]
    public void HasPlayerAlreadyClaimed_ShouldReturnFalse_WhenPlayerHasNotClaimed()
    {
        // Act
        var result = claimTrackerService.HasPlayerAlreadyClaimed(GameType.TopLine, Guid.NewGuid());

        // Assert
        result.ShouldBeFalse();
    }
}

public class TestPriorityAttribute : Attribute
{
    public int Priority { get; }
    public TestPriorityAttribute(int priority) => Priority = priority;
}

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var sortedTests = testCases.OrderBy(test =>
        {
            var priorityAttr = test.TestMethod.Method.GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName).FirstOrDefault();
            return priorityAttr == null ? 0 : priorityAttr.GetNamedArgument<int>("Priority");
        });

        return sortedTests;
    }
}
