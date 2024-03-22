using ConwaysGameofLife.Application.Services;
using ConwaysGameofLife.Domain.DTOS;
using Shouldly;
using Xunit;

namespace ConwaysGameofLife.Tests.Application.Services
{
    public class RulesServiceTest
    {
        [Theory]
        [InlineData(true, 1, false)]
        [InlineData(true, 2, true)]
        [InlineData(true, 3, true)]
        [InlineData(true, 4, false)]
        [InlineData(false, 1, false)]
        [InlineData(false, 2, false)]
        [InlineData(false, 3, true)]
        [InlineData(false, 4, false)]
        public void CanAddCell_should_return_expected_values(bool isAlive, int numNeighbors, bool expected)
        {
            // Arrange
            var target = new RulesService();

            // Act
            var actual = target.CanAddCell(isAlive, numNeighbors);

            // Assert
            actual.ShouldBe(expected);
        }


        [Theory]
        [MemberData(nameof(HasConcludedInputs))]
        public void HasConcluded_should_return_expected_values(BoardState previousState, BoardState currentState, bool expected)
        {
            // Arrange
            var target = new RulesService();

            // Act
            var actual = target.HasConcluded(previousState, currentState);

            // Assert
            actual.ShouldBe(expected);
        }

        public static IEnumerable<object[]> HasConcludedInputs()
        {
            // Different board states and current non empty : false
            yield return new object[] {
                new BoardState
                {
                    { new(1,1),true },
                    { new(1,2),true }
                },
                new BoardState
                {
                    { new (2,2), true},
                    { new (2,3), true}
                },
                false
            };

            // Boards have the same cells, but in one they are alive, but other they are dead and an extra alive: false
            yield return new object[] {
                new BoardState
                {
                    { new(1,1),true },
                    { new(1,2),true }
                },
                new BoardState
                {
                    { new(1,1),false },
                    { new(1,2),false },
                    { new(1,3),true }
                },
                false
            };

            // Boards have the same cells : true
            yield return new object[] {
                new BoardState
                {
                    { new(1,1),true },
                    { new(1,2),true }
                },
                new BoardState
                {
                    { new(1,1),true },
                    { new(1,2),true }
                },
                true
            };
            // Boards have the same cells, dead cells included and different orders : true
            yield return new object[] {
                new BoardState
                {
                    { new(1,2),true },
                    { new(3,3),false },
                    { new(1,1),true },
                },
                new BoardState
                {
                    { new(1,1),true },
                    { new(1,2),true }
                },
                true
            };
            // Board 2 is empty : true
            yield return new object[] {
                new BoardState
                {
                    { new(1,2),true },
                    { new(3,3),false },
                    { new(1,1),true },
                },
                new BoardState
                {
                },
                true
            };
            // Board 2 contais only dead cells: true
            yield return new object[] {
                new BoardState
                {
                    { new(1,2),true },
                    { new(3,3),false },
                    { new(1,1),true },
                },
                new BoardState
                {
                    { new(1,1),false },
                    { new(1,2),false }
                },
                true
            };
            // Board 1 is already empty : true
            yield return new object[] {
                new BoardState
                {
                },
                new BoardState
                {
                },
                true
            };
            // Board 1 contains only dead cells : true
            yield return new object[] {
                new BoardState
                {
                    { new(1,1),false },
                    { new(1,2),false }
                },
                new BoardState
                {
                },
                true
            };
        }
    }
}
