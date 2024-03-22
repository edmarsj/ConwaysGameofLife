using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class CorruptedBoardException : KnownException
    {
        public CorruptedBoardException() : base("corrupted_board", System.Net.HttpStatusCode.InternalServerError)
        {

        }
    }
}
