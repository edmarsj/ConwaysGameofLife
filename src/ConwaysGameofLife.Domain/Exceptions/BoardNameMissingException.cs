using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class BoardNameMissingException : KnownException
    {
        public BoardNameMissingException():base("board_name_missing", System.Net.HttpStatusCode.BadRequest)
        {
            
        }
    }
}
