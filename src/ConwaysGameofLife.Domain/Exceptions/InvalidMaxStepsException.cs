using ConwaysGameofLife.Core.Exceptions;

namespace ConwaysGameofLife.Domain.Exceptions
{
    public class InvalidMaxStepsException : KnownException
    {
        public InvalidMaxStepsException() : base("maxSteps_must_be_greater_than_zero", System.Net.HttpStatusCode.BadRequest)
        {

        }
    }
}
