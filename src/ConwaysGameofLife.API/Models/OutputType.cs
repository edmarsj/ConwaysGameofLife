namespace ConwaysGameofLife.API.Models
{
    public enum OutputType
    {
        /// <summary>
        /// Output using ascii characters
        /// </summary>
        Ascii = 0,       
        /// <summary>
        /// Output using a matrix, where 0 represents a dead cell and 1 represents an alive cell
        /// </summary>
        Matrix = 1        
    }
}
