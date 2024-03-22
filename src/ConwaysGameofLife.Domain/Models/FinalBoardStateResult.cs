using ConwaysGameofLife.Domain.DTOS;

namespace ConwaysGameofLife.Domain.Models
{
    public record FinalBoardStateResult(string BoardName, int NumAttempts, BoardState FinalState, long ElapsedMilliseconds)
    {
        public bool Error => FinalState == null;
    }
}
