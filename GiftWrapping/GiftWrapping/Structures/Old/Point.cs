using System;
using System.Collections;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class Point:IPoint
    {
        static private double _eps = 1e-8;

        private readonly double[] coordinates;

        public Point(params double[] coordinates)
        {
            this.coordinates = coordinates;
        }

        public Point(int length)
        {
            this.coordinates = new double[length];
        }

        public double this[int index]
        {
            get => coordinates[index];
            set => coordinates[index] = value;
        }

        public int Dimension()
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

        public int CompareTo(IPoint other)
        {
            for (int i = 0; i < this.coordinates.Length; i++)
            {
                if (Math.Abs(this[i] - other[i]) <_eps)
                {
                   continue;
                }
                else if (this[i] - other[i] < 0)
                {
                    return -1;
                }
                else
                {
                    return +1;
                }
            }

            return 0;
        }
    }
}