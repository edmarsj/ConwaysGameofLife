using ConwaysGameofLife.Domain.DTOS;
using Newtonsoft.Json;

namespace ConwaysGameofLife.API.Models
{
    public abstract class BoardStateResult
    {
        public string BoardName { get; set; }
        public int NumGenerations { get; set; }
        public string Took { get; set; }
        public bool FinishedOnError { get; set; }

        public static BoardStateResult From(string boardName, BoardState state, OutputType outputType, long elapsedMilliseconds, int numAttempts, bool finishedOnError)
        {
            var states = new BoardState[] { state };

            BoardStateResult result = From(boardName, states, elapsedMilliseconds, outputType);

            result.NumGenerations = numAttempts;
            result.FinishedOnError = finishedOnError;

            return result;
        }

        public static BoardStateResult From(string boardName, IEnumerable<BoardState> states, long elapsedMilliseconds, OutputType outputType)
        {
            BoardStateResult result = null;
            var filteredStates = states.Where(m => m != null);

            if (outputType == OutputType.Ascii)
            {
                result = new BoardStateResult<string[]>
                {
                    States = filteredStates.Select(m => m.ToAscii())
                };
            }
            else
            {
                result = new BoardStateResult<int[,]>
                {
                    States = filteredStates.Select(m => m.ToMatrix())
                };
            }

            result.BoardName = boardName;
            result.NumGenerations = filteredStates.Count();
            result.Took = $"{elapsedMilliseconds} ms"; ;

            return result;
        }
    }
    public class BoardStateResult<T> : BoardStateResult
    {
        // Execute Order 66. 
        //    - Sidious,Darth
        [JsonProperty(Order = 66)]
        public IEnumerable<T> States { get; set; }
    }
}
