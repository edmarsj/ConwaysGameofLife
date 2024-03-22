using Xunit;
using Shouldly;
using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Exceptions;
using System.Text;

namespace ConwaysGameofLife.Tests.Domain.DTOS
{
    public class BoardStateTests
    {

        [Fact]
        public void AddOrUpdateCell_should_add_new_element()
        {
            // Arrange
            var target = new BoardState();
            var key = new Cell(1, 2);

            // Act
            target.AddOrUpdateCell(key, true);

            // Assert            
            target.ShouldContainKeyAndValue(key, true);
        }

        [Fact]
        public void AddOrUpdateCell_should_not_update_element_if_current_is_alive()
        {
            // Arrange
            var target = new BoardState();
            var key = new Cell(1, 2);
            target.Add(key, true);

            // Act
            target.AddOrUpdateCell(key, false);

            // Assert            
            target.ShouldContainKeyAndValue(key, true);
        }

        [Fact]
        public void AddOrUpdateCell_should_update_element_if_current_not_alive()
        {
            // Arrange
            var target = new BoardState();
            var key = new Cell(1, 2);
            target.Add(key, true);

            // Act
            target.AddOrUpdateCell(key, false);

            // Assert            
            target.ShouldContainKeyAndValue(key, true);
        }

        [Theory]
        [MemberData(nameof(GetNumAliveNeighborsInputs))]
        public void GetNumAliveNeighbors_should_return_the_count_of_all_alive_neighbors(Cell key, int expected)
        {
            // Arrange
            var target = new BoardState
            {
                { new Cell(3, 3), false },
                { new Cell(2, 2), true },
                { new Cell(3, 2), true },
                { new Cell(4, 2), true },
                { new Cell(2, 3), false },
                { new Cell(4, 3), false },
                { new Cell(2, 4), true },
                { new Cell(3, 4), true },
                { new Cell(4, 4), true }
            };

            // Act
            var count = target.GetNumAliveNeighbors(key);

            // Assert
            count.ShouldBe(expected);
        }

        public static IEnumerable<object[]> GetNumAliveNeighborsInputs()
        {
            yield return new object[] { new Cell(3, 3), 6 };
            yield return new object[] { new Cell(3, 2), 2 };
            yield return new object[] { new Cell(3, 1), 3 };
            yield return new object[] { new Cell(1, 1), 1 };
        }

        [Theory]
        [InlineData(@"0000
                      1111
                      11")]
        [InlineData(@"0000
                      1111
                      11111")]
        public void FromFileContent_should_throw_exception_on_invalid_board(string fileContent)
        {
            Should.Throw<InvalidBoardFileException>(() => BoardState.FromFileContent(fileContent));
        }

        [Fact]
        public void FromFileContent_should_throw_exception_when_file_have_just_one_line()
        {
            Should.Throw<InvalidBoardFileException>(() => BoardState.FromFileContent("0000111111"));
        }

        [Theory]
        [InlineData(@"0000
                      1111
                      1111")]
        public void FromFileContent_should_accept_a_valid_board(string fileContent)
        {
            Should.NotThrow(() => BoardState.FromFileContent(fileContent));
        }

        [Fact]
        public void FromMatrix_should_load_data_correctly()
        {
            // Arrange
            var input = new int[,]
            {
                { 0,0,1 },
                { 0,1,1 }
            };

            // Act
            var current = BoardState.FromMatrix(input);

            // Assert
            current.ShouldContainKeyAndValue(new Cell(0, 0), false);
            current.ShouldContainKeyAndValue(new Cell(0, 1), false);
            current.ShouldContainKeyAndValue(new Cell(0, 2), true);
            current.ShouldContainKeyAndValue(new Cell(1, 0), false);
            current.ShouldContainKeyAndValue(new Cell(1, 1), true);
            current.ShouldContainKeyAndValue(new Cell(1, 2), true);
        }

        [Fact]
        public void ToMatrix_should_generate_data_correctly()
        {
            // Arrange
            var target = new int[,]
            {
                { 0,0,1 },
                { 0,1,1 }
            };

            var state = new BoardState
            {
                { new Cell(0, 0), false},
                { new Cell(1, 0), false},
                { new Cell(2, 0), true},
                { new Cell(0, 1), false},
                { new Cell(1, 1), true},
                { new Cell(2, 1), true}
            };

            // Act
            var current = state.ToMatrix();

            // Assert
            target.ShouldBeEquivalentTo(current);
        }

        [Fact]
        public void ToAscii_should_generate_data_correctly()
        {
            // Arrange
            var target = new string[]
            {
                "▓▓▓▓▓▓▓",
                "▓     ▓",
                "▓   ▓ ▓",
                "▓  ▓▓ ▓",
                "▓     ▓",
                "▓▓▓▓▓▓▓"
            };            

            var state = new BoardState
            {
                { new Cell(0, 0), false},
                { new Cell(1, 0), false},
                { new Cell(2, 0), true},
                { new Cell(0, 1), false},
                { new Cell(1, 1), true},
                { new Cell(2, 1), true}
            };

            // Act
            var current = state.ToAscii();

            // Assert
            for(var i =0; i < target.Length;i++)
            {
                current[i].ShouldBe(target[i]);
            }            
        }
    }
}
