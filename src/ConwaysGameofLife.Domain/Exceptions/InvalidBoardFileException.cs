using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class InvalidBoardFileException : KnownException
    {
        public InvalidBoardFileException() : base("invalid_file", System.Net.HttpStatusCode.BadRequest)
        {

        }
    }
}
