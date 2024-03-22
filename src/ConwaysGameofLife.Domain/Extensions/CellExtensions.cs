using ConwaysGameofLife.Domain.DTOS;

namespace ConwaysGameofLife.Domain.Extensions
{
    public static class CellExtensions
    {
        public static Cell TopLeft(this Cell instance) => new(instance.X - 1, instance.Y - 1);
        public static Cell Top(this Cell instance) => new(instance.X, instance.Y - 1);
        public static Cell TopRight(this Cell instance) => new(instance.X + 1, instance.Y - 1);

        public static Cell Left(this Cell instance) => new(instance.X - 1, instance.Y);
        public static Cell Right(this Cell instance) => new(instance.X + 1, instance.Y);

        public static Cell BottomLeft(this Cell instance) => new(instance.X - 1, instance.Y + 1);
        public static Cell Bottom(this Cell instance) => new(instance.X, instance.Y + 1);
        public static Cell BottomRight(this Cell instance) => new(instance.X + 1, instance.Y + 1);

        public static Cell[] GetNeighbors(this Cell instance) => new[]
        {
            instance.TopLeft(), instance.Top(), instance.TopRight(),
            instance.Left(), instance.Right(),
            instance.BottomLeft(), instance.Bottom(), instance.BottomRight()
        };
    }
}
