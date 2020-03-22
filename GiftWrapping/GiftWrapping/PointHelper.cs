using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class PointHelper
    {
        public static bool HaveSameDimension(this IList<Point> points)
        {
            return points.All(v => v.Dim == points[0].Dim);
        }

        public static Vector[] ToVectors(this IList<Point> points)
        {
            Vector[] vectors = new Vector[points.Count - 1];
            Point firstPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                vectors[i - 1] = Point.ToVector(firstPoint, points[i]);
            }

            return vectors;
        }
    }
}