using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class BoardNotFoundException : KnownException
    {
        public BoardNotFoundException() : base("board_not_found", System.Net.HttpStatusCode.NotFound)
        {

        }
    }
}
