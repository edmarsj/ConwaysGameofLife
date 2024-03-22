using ConwaysGameofLife.Application.Services;
using ConwaysGameofLife.Domain.DTOS;
using Moq;
using Shouldly;
using Xunit;

namespace ConwaysGameofLife.Tests.Application.Services
{
    public class GameOfLifeServiceTests
    {
        [Fact]
        public void GetNextGeneration_should_add_cells_to_next_generation()
        {
            // Arrange
            var rulesServiceMock = Mock.Of<IRulesService>(m =>
                                    m.CanAddCell(It.IsAny<bool>(), It.IsAny<int>()) == true);

            var target = new GameOfLifeService(rulesServiceMock);

            var currentState = new BoardState
            {
                { new Cell(0,0), true }
            };

            // Act
            var actual = target.GetNextGeneration(currentState);

            // Assert            
            actual.ShouldContainKey(new Cell(-1, -1));
            actual.ShouldContainKey(new Cell(0, -1));
            actual.ShouldContainKey(new Cell(1, -1));
            actual.ShouldContainKey(new Cell(-1, 0));
            actual.ShouldContainKey(new Cell(0, 0));
            actual.ShouldContainKey(new Cell(1, 0));
            actual.ShouldContainKey(new Cell(-1, 1));
            actual.ShouldContainKey(new Cell(0, 1));
            actual.ShouldContainKey(new Cell(1, 1));
        }

        [Fact]
        public void GetNextGeneration_should_add_cells_to_next_generation_with_correct_alive_values()
        {
            // Arrange
            var rulesServiceMock = Mock.Of<IRulesService>(m =>
                                    m.CanAddCell(It.IsAny<bool>(), It.IsAny<int>()) == true);

            var target = new GameOfLifeService(rulesServiceMock);

            var currentState = new BoardState
            {
                { new Cell(0,0), true }
            };

            // Act
            var actual = target.GetNextGeneration(currentState);

            // Assert            
            actual.ShouldContainKeyAndValue(new Cell(-1, -1), false);
            actual.ShouldContainKeyAndValue(new Cell(0, -1), false);
            actual.ShouldContainKeyAndValue(new Cell(1, -1), false);
            actual.ShouldContainKeyAndValue(new Cell(-1, 0), false);
            actual.ShouldContainKeyAndValue(new Cell(0, 0), true);
            actual.ShouldContainKeyAndValue(new Cell(1, 0), false);
            actual.ShouldContainKeyAndValue(new Cell(-1, 1), false);
            actual.ShouldContainKeyAndValue(new Cell(0, 1), false);
            actual.ShouldContainKeyAndValue(new Cell(1, 1), false);
        }

        [Fact]
        public void GetNextGeneration_should_NOT_add_cells_to_next_generation()
        {
            // Arrange
            var rulesServiceMock = Mock.Of<IRulesService>(m =>
                                    m.CanAddCell(It.IsAny<bool>(), It.IsAny<int>()) == false);

            var target = new GameOfLifeService(rulesServiceMock);

            var currentState = new BoardState
            {
                { new Cell(0,0), true }
            };

            // Act
            var actual = target.GetNextGeneration(currentState);

            // Assert            
            actual.ShouldNotContainKey(new Cell(-1, -1));
            actual.ShouldNotContainKey(new Cell(0, -1));
            actual.ShouldNotContainKey(new Cell(1, -1));
            actual.ShouldNotContainKey(new Cell(-1, 0));
            actual.ShouldNotContainKey(new Cell(0, 0));
            actual.ShouldNotContainKey(new Cell(1, 0));
            actual.ShouldNotContainKey(new Cell(-1, 1));
            actual.ShouldNotContainKey(new Cell(0, 1));
            actual.ShouldNotContainKey(new Cell(1, 1));
        }

        [Fact]
        public void GetFinalState_should_return_null_state_after_3_failed_attempts()
        {
            // Arrange
            var rulesServiceMock = Mock.Of<IRulesService>(m =>
                                    m.HasConcluded(It.IsAny<BoardState>(), It.IsAny<BoardState>()) == false);

            var target = new GameOfLifeService(rulesServiceMock);

            var currentState = new BoardState
            {
                { new Cell(0,0), true }
            };

            // Act
            var actual = target.GetFinalState(currentState, maxGenerations: 3);

            // Assert
            actual.finalState.ShouldBeNull();
            actual.numGenerations.ShouldBe(3);
        }

        [Fact]
        public void GetFinalState_should_return_state_after_2_failed_attempts()
        {
            // Arrange
            var rulesServiceMock = new Mock<IRulesService>();
            rulesServiceMock.SetupSequence(m => m.HasConcluded(It.IsAny<BoardState>(), It.IsAny<BoardState>()))
                            .Returns(false)
                            .Returns(false)
                            .Returns(true);

            var target = new GameOfLifeService(rulesServiceMock.Object);

            var currentState = new BoardState
            {
                { new Cell(0,0), true }
            };

            // Act
            var actual = target.GetFinalState(currentState, maxGenerations: 100);

            // Assert
            actual.finalState.ShouldNotBeNull();
            actual.numGenerations.ShouldBe(2);
        }


    }
}
