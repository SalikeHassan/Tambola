using System.Collections.Generic;
using FakeItEasy;
using Shouldly;
using Tambola.Api.src.Application.Validators;
using Tambola.Api.src.Domain;
using Xunit;
using Tambola.Api.src.Application.Strategies.Factory;
using Tambola.Api.src.Application.Strategies;

namespace Tambola.Api.Test
{
    public class ClaimValidatorTests
    {
        private readonly IGameStrategyFactory fakeStrategyFactory;
        private readonly IGameStrategy fakeStrategy;
        private readonly ClaimValidator claimValidator;

        public ClaimValidatorTests()
        {
            fakeStrategyFactory = A.Fake<IGameStrategyFactory>();
            fakeStrategy = A.Fake<IGameStrategy>();

            A.CallTo(() => fakeStrategyFactory.Create(A<GameType>._))
                .Returns(fakeStrategy);

            claimValidator = new ClaimValidator(fakeStrategyFactory);
        }

        [Fact]
        public void ValidateClaim_ShouldReturnTrue_WhenWinningConditionIsMet()
        {
            var ticket = Ticket.Create(new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            });

            var announcedNumbers = new List<int> { 90, 4, 46, 63, 89, 16, 76, 48 };

            A.CallTo(() => fakeStrategy.IsWinningCondition(ticket, announcedNumbers)).Returns(true);
            A.CallTo(() => fakeStrategy.IsWinningCondition(ticket, A<List<int>>.That.Matches(n => n.Count == announcedNumbers.Count - 1)))
                .Returns(false);

            var result = claimValidator.ValidateClaim(ticket, announcedNumbers, GameType.TopLine);

            result.ShouldBeTrue();
        }

        [Fact]
        public void ValidateClaim_ShouldReturnFalse_WhenWinningConditionWasAlreadyMet()
        {
            var ticket = Ticket.Create(new int?[][]
            {
                new int?[] { 4, 16, null, null, 48, null, 63, 76, null },
                new int?[] { 7, null, 23, 38, null, 52, null, null, 80 },
                new int?[] { 9, null, 25, null, null, 56, 64, null, 83 }
            });

            var announcedNumbers = new List<int> { 90, 4, 46, 63, 89, 16, 76, 48, 12 };

            A.CallTo(() => fakeStrategy.IsWinningCondition(ticket, announcedNumbers)).Returns(true);
            A.CallTo(() => fakeStrategy.IsWinningCondition(ticket, A<List<int>>.That.Matches(n => n.Count == announcedNumbers.Count - 1)))
                .Returns(true);

            var result = claimValidator.ValidateClaim(ticket, announcedNumbers, GameType.TopLine);

            result.ShouldBeFalse();
        }
    }
}
