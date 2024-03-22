using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class BoardInitialStateMissingException : KnownException
    {
        public BoardInitialStateMissingException() : base("board_initial_state_missing", System.Net.HttpStatusCode.BadRequest)
        {

        }
    }
}
