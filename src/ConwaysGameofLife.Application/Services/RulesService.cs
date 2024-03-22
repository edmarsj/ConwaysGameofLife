using ConwaysGameofLife.Domain.DTOS;

namespace ConwaysGameofLife.Application.Services
{
    public class RulesService : IRulesService
    {
        public bool CanAddCell(bool isAlive, int numNeighbors)
        {
            var addCell = false;
            // Any live cell with two or three live neighbors lives on to the next generation.
            if (isAlive && (numNeighbors == 3 || numNeighbors == 2))
            {
                addCell = true;
            }

            // Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction
            if (!isAlive && numNeighbors == 3)
            {
                addCell = true;
            }

            return addCell;
        }

        /// <summary>
        /// Checks if one of the following conditions is true:
        /// 1 - The current generation doesn't have any alive cell
        /// 2 - The current generation is equal to the previous one
        /// </summary>
        /// <param name="previousState">The previous generation</param>
        /// <param name="currentState">The current generation</param>
        /// <returns>True if one of the conditions has been met</returns>
        public bool HasConcluded(BoardState previousState, BoardState currentState)
        {
            // Has any alive cell
            if (!currentState.Any(m => m.Value))
            {
                return true;
            }

            // Compares if the two states have the same number of alive cells
            if (previousState.Count(m => m.Value) == currentState.Count(m => m.Value))
            {
                var aliveCellsBefore = previousState.Where(m => m.Value).Select(m => m.Key).ToList();
                var aliveCellsCurrent = currentState.Where(m => m.Value).Select(m => m.Key).ToList();

                // Sort the data leveraging the cells' equality comparer
                // The order of the cells in the state may vary because of the parallel processing, but it should not matter as the 
                // visual representation is the same                
                aliveCellsBefore.Sort();
                aliveCellsCurrent.Sort();

                return aliveCellsCurrent.SequenceEqual(aliveCellsBefore);
            }

            // No condition has been met, board is not concluded
            return false;
        }
    }
}
