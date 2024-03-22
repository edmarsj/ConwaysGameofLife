namespace ConwaysGameofLife.API.Models
{
    public record UploadBoardRequestModel(string BoardName, IFormFile BoardData)
    {
    }
}
