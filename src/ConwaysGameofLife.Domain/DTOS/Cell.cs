namespace ConwaysGameofLife.Domain.DTOS
{
    public struct Cell : IComparable<Cell>
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public readonly int CompareTo(Cell other)
        {
            var currentIndex = (Y + 1) * X;
            var otherIndex = (other.Y + 1) * other.X;

            return currentIndex.CompareTo(otherIndex);
        }
    }
}
