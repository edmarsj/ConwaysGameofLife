using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class CorruptedBoardSnapshotException : KnownException
    {
        public CorruptedBoardSnapshotException() : base("corrupted_snapshot", System.Net.HttpStatusCode.InternalServerError)
        {

        }
    }
}
