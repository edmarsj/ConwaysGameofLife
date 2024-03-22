using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class InvalidNumStatesException : KnownException
    {
        public InvalidNumStatesException() : base("numStates_must_be_greater_than_zero", System.Net.HttpStatusCode.BadRequest)
        {

        }
    }
}
