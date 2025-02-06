using System;
using System.Collections.Generic;
using FakeItEasy;
using Shouldly;
using Xunit;
using Tambola.Api.src.Application.Common;
using Tambola.Api.src.Application.Strategies.Factory;
using Tambola.Api.src.Domain;
using Tambola.Api.src.Application.Strategies;

namespace Tambola.Api.Test.Application.Strategies.Factory;

public class GameStrategyFactoryTests
{
    private readonly List<IGameStrategy> strategies;
    private readonly GameStrategyFactory gameStrategyFactory;

    public GameStrategyFactoryTests()
    {
        var topLineStrategy = A.Fake<IGameStrategy>();
        A.CallTo(() => topLineStrategy.Key).Returns(GameType.TopLine);

        var middleLineStrategy = A.Fake<IGameStrategy>();
        A.CallTo(() => middleLineStrategy.Key).Returns(GameType.MiddleLine);

        var earlyFiveStrategy = A.Fake<IGameStrategy>();
        A.CallTo(() => earlyFiveStrategy.Key).Returns(GameType.EarlyFive);

        var bottomLineStrategy = A.Fake<IGameStrategy>();
        A.CallTo(() => bottomLineStrategy.Key).Returns(GameType.BottomLine);

        var fullHouseStrategy = A.Fake<IGameStrategy>();
        A.CallTo(() => fullHouseStrategy.Key).Returns(GameType.FullHouse);

        strategies = new List<IGameStrategy>
            {
                topLineStrategy,
                middleLineStrategy,
                earlyFiveStrategy,
                bottomLineStrategy,
                fullHouseStrategy
            };

        gameStrategyFactory = new GameStrategyFactory(strategies);
    }

    [Theory]
    [InlineData(GameType.TopLine)]
    [InlineData(GameType.MiddleLine)]
    [InlineData(GameType.EarlyFive)]
    [InlineData(GameType.BottomLine)]
    [InlineData(GameType.FullHouse)]
    public void Create_ShouldReturnCorrectStrategy_ForValidGameType(GameType gameType)
    {
        // Act
        var result = gameStrategyFactory.Create(gameType);

        // Assert
        result.ShouldNotBeNull();
        result.Key.ShouldBe(gameType);
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_ForInvalidGameType()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() =>
        {
            gameStrategyFactory.Create((GameType)999);
        }).Message.ShouldBe(Constants.InvalidGameTypeErrMsg);
    }
}