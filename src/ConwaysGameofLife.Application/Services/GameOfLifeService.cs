using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Extensions;

namespace ConwaysGameofLife.Application.Services
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IRulesService _rulesService;
        public GameOfLifeService(IRulesService cellEvaluatorService)
        {
            _rulesService = cellEvaluatorService;
        }

        public BoardState GetBoardGeneration(BoardState state, int generation)
        {
            var currentState = state;
            for (var i = 0; i < generation; i++)
            {
                currentState = GetNextGeneration(currentState);
            }
            return currentState;
        }

        public IEnumerable<BoardState> GetBoardGenerations(BoardState state, int numGenerations)
        {
            var generations = new List<BoardState>();

            var currentState = state;
            for (var i = 0; i < numGenerations; i++)
            {
                currentState = GetNextGeneration(currentState);
                generations.Add(currentState);
            }
            return generations;
        }

        public (BoardState finalState, int numGenerations) GetFinalState(BoardState state, int maxGenerations)
        {
            var currentState = state;
            for (var i = 0; i < maxGenerations; i++)
            {
                var nextState = GetNextGeneration(currentState);

                if (_rulesService.HasConcluded(currentState, nextState))
                {
                    return (currentState, i);
                }
                currentState = nextState;
            }

            // If we get here we return null and report back the number of generations. 
            return (null, maxGenerations);
        }

        public BoardState GetNextGeneration(BoardState currentState)
        {
            var nextState = new BoardState();

            // Evaluates each cell
            foreach(var cellState in currentState)            
            {
                var isAlive = cellState.Value;
                var numNeighbors = currentState.GetNumAliveNeighbors(cellState.Key);

                if (_rulesService.CanAddCell(isAlive, numNeighbors))
                {
                    // We add the live cell and its possible empty neighbors 
                    var cell = cellState.Key;
                    nextState.AddOrUpdateCell(cell, true);
                    var neighbors = cellState.Key.GetNeighbors();
                    foreach (var n in neighbors)
                    {
                        nextState.AddOrUpdateCell(n, false);
                    }
                }
            };

            return nextState;
        }
    }





}
