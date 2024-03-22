using ConwaysGameofLife.Domain.DTOS;

namespace ConwaysGameofLife.Application.Services
{
    public interface IGameOfLifeService
    {
        BoardState GetBoardGeneration(BoardState state, int interation);
        IEnumerable<BoardState> GetBoardGenerations(BoardState state, int numInterations);
        (BoardState finalState, int numGenerations) GetFinalState(BoardState state, int maxGenerations);
        BoardState GetNextGeneration(BoardState currentState);
    }
}