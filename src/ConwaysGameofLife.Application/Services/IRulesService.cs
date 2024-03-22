using ConwaysGameofLife.Domain.DTOS;

namespace ConwaysGameofLife.Application.Services
{
    public interface IRulesService
    {
        bool CanAddCell(bool isAlive, int numNeighbors);
        bool HasConcluded(BoardState previousState, BoardState currentState);
    }
}