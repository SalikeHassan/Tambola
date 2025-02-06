using System;
using System.Collections.Generic;
using FakeItEasy;
using Shouldly;
using Xunit;
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
            // Arrange: Create fakes for strategies
            var topLineStrategy = A.Fake<IGameStrategy>();
            A.CallTo(() => topLineStrategy.Key).Returns(GameType.TopLine);

            var middleLineStrategy = A.Fake<IGameStrategy>();
            A.CallTo(() => middleLineStrategy.Key).Returns(GameType.MiddleLine);

            strategies = new List<IGameStrategy>
            {
                topLineStrategy,
                middleLineStrategy
            };

            // Initialize factory with the fake strategies
            gameStrategyFactory = new GameStrategyFactory(strategies);
        }

        [Fact]
        public void Create_ShouldReturnCorrectStrategy_ForValidGameType()
        {
            // Act
            var result = gameStrategyFactory.Create(GameType.TopLine);

            // Assert
            result.ShouldNotBeNull();
            result.Key.ShouldBe(GameType.TopLine);
        }

        [Fact]
        public void Create_ShouldThrowArgumentException_ForInvalidGameType()
        {
            // Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                gameStrategyFactory.Create(GameType.FullHouse);
            }).Message.ShouldBe("Invalid notification type");
        }
    }