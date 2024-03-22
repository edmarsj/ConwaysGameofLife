using ConwaysGameofLife.Domain.DTOS;

namespace ConwaysGameofLife.Domain.Models
{
    public record BoardQueryResult(string BoardName, IEnumerable<BoardState> States, long ElapsedMilliseconds);    
}
