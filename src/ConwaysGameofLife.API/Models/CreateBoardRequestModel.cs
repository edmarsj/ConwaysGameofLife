namespace ConwaysGameofLife.API.Models
{
    public record CreateBoardRequestModel(string BoardName, int[,] InitialState);    
}
