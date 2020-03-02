using System.Collections;

namespace GiftWrapping.Structures
{
    public class Point:IPoint
    {
        private readonly double[] coordinates;

        public Point(params double[] coordinates)
        {
            this.coordinates = coordinates;
        }

        public double this[int index]
        {
            get => coordinates[index];
            set => coordinates[index] = value;
        }

        public int GetDimension()
        {
            return coordinates.Length;
        }

        public double[] ToArray()
        {
            return coordinates;
        }


        public bool Equals(IPoint other)
        {
            return coordinates.Equals(other.ToArray());
        }
    }
}