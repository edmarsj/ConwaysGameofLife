using ConwaysGameofLife.Domain.Exceptions;
using ConwaysGameofLife.Domain.Extensions;
using Newtonsoft.Json;
using System.Text;

namespace ConwaysGameofLife.Domain.DTOS
{
    /// <summary>
    /// Represents the state of a single generation
    /// </summary>
    public class BoardState : Dictionary<Cell, bool>
    {
        /// <summary>
        /// Creates a board state from a file.
        /// All lines in the file must have the same column count and the file needs to have at least two lines
        /// </summary>
        /// <param name="fileContent">The contents of the file</param>
        /// <returns>The board state</returns>

        public static BoardState FromFileContent(string fileContent)
        {
            var lines = fileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // We want files with more than one line
            if (lines.Length == 1)
            {
                throw new InvalidBoardFileException();
            }

            // Uses the first row to identify the number of collumns
            var numCols = lines.First().Length;

            var state = new int[numCols, lines.Length];

            // Validation
            foreach (var line in lines)
            {
                if (line.Length != numCols)
                {
                    throw new InvalidBoardFileException();
                }
            }

            // Load the data
            for (var x = 0; x < numCols; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    state[x, y] = lines[y][x] == '1' ? 1 : 0;
                }
            }

            return FromMatrix(state);
        }

        /// <summary>
        /// Creates a board state from a integer matrix
        /// </summary>
        /// <param name="state">The new state</param>
        /// <returns>The board state</returns>
        public static BoardState FromMatrix(int[,] state)
        {
            var currentState = new BoardState();

            for (int x = 0; x < state.GetLength(0); x++)
            {
                for (int y = 0; y < state.GetLength(1); y++)
                {
                    currentState.AddOrUpdateCell(new(x, y), state[x, y] == 1);
                }
            }

            return currentState;
        }

        /// <summary>
        /// Converts the board to an integer matrix
        /// </summary>
        /// <returns>The resulting matrix</returns>
        public int[,] ToMatrix()
        {
            if (!this.Any())
            {
                return new int[,] { { 0 } };
            }

            // boundaries
            var minX = Math.Min(0, Keys.Min(m => m.X)) * -1;
            var minY = Math.Min(0, Keys.Min(m => m.Y)) * -1;
            var maxX = Keys.Max(m => m.X) + 1 + minX;
            var maxY = Keys.Max(m => m.Y) + 1 + minY;


            var result = new int[maxY, maxX];

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (TryGetValue(new Cell(x - minX, y - minY), out var value))
                    {
                        result[y, x] = value ? 1 : 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generates an Ascii representation of the board. 
        /// The representation has a border and extra space to highlight the boards content
        /// </summary>
        /// <returns>The ascii representation</returns>
        public string[] ToAscii()
        {
            const char block = '▓';

            var result = new List<string>();
            var matrix = ToMatrix();

            // Add borders and extra row on the top
            result.Add(new string(block, matrix.GetLength(1) + 4));
            result.Add(block + new string(' ', matrix.GetLength(1) + 2) + block);

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                var stringBuilder = new StringBuilder();

                // Adds border and extra space before
                stringBuilder.Append(block + " ");

                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    stringBuilder.Append(matrix[x, y] == 1 ? block : " ");
                }

                var remainingSpaces = Math.Max(0, matrix.GetLength(1) - stringBuilder.Length);

                // Adds border and extra space after
                stringBuilder.Append(' ', remainingSpaces + 1);
                stringBuilder.Append(block);
                result.Add(stringBuilder.ToString());
            }

            result.Add(block + new string(' ', matrix.GetLength(1) + 2) + block);
            // Add borders and extra row to the bottom
            result.Add(new string(block, matrix.GetLength(1) + 4));

            return result.ToArray();
        }

        /// <summary>
        /// Returns a serialized version of the data
        /// </summary>
        /// <returns>The serialized data</returns>
        public string ToSnapshotString()
        {
            return JsonConvert.SerializeObject(ToListOnlyAlive());
        }

        /// <summary>
        /// Returns a list with only the alive cells
        /// </summary>
        /// <returns>The list with alive cells</returns>
        public IEnumerable<Cell> ToListOnlyAlive()
        {
            return this.Where(m => m.Value).Select(m => m.Key);
        }

        /// <summary>
        /// Creates a board state from its serialized representation
        /// </summary>
        /// <param name="snapshot">The json serialized data</param>
        /// <returns>The board</returns>
        /// <exception cref="CorruptedBoardSnapshotException">If the data can't be deserialized</exception>
        public static BoardState FromSnapshotString(string snapshot)
        {
            try
            {
                var cells = JsonConvert.DeserializeObject<List<Cell>>(snapshot);
                return FromListOnlyAlive(cells);
            }
            catch (JsonSerializationException)
            {
                throw new CorruptedBoardSnapshotException();
            }
        }

        /// <summary>
        /// Creates a board from a list of alive cells
        /// </summary>
        /// <param name="list">The list of alive cells</param>
        /// <returns>The board</returns>
        public static BoardState FromListOnlyAlive(IEnumerable<Cell> list)
        {
            var newState = new BoardState();
            foreach (var cell in list)
            {
                newState.AddOrUpdateCell(cell, true);
                foreach (var n in cell.GetNeighbors())
                {
                    newState.AddOrUpdateCell(n, false);
                }
            }

            return newState;
        }
        
        /// <summary>
        /// Add a new cell to the state or update an existing cell setting it as alive if it is dead
        /// This function does not update an alive cell to dead. 
        /// Dead cells need to be removed using Remove
        /// </summary>
        /// <param name="cell">The key for the new cell</param>
        /// <param name="alive">The state for this cell</param>
        public void AddOrUpdateCell(Cell cell, bool alive)
        {
            if (!ContainsKey(cell))
            {
                Add(cell, alive);
                return;
            }

            this[cell] |= alive;
        }
        /// <summary>
        /// Returns the number of alive Neighbors of a cell
        /// </summary>
        /// <param name="cell">The cell</param>
        /// <returns>The number of alive neighbors</returns>
        public int GetNumAliveNeighbors(Cell cell)
        {
            // Get info from all existing neighbors 
            return cell.GetNeighbors().Where(m => TryGetValue(m, out var alive) && alive).Count();
        }
    }
}
