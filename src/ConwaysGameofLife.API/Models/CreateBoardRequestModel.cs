namespace ConwaysGameofLife.API.Models
{
    public record CreateBoardRequestModel(string Name, int[,] InitialState);    
}
