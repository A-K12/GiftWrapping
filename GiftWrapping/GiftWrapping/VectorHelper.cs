using System;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class VectorHelper
    {
        public static Point CreateVector(IPoint startPoint, IPoint endPoint)
        {
            var vertor = new Point(startPoint.Dimension());
            for (int i = 0; i < startPoint.Dimension(); i++)
            {
                vertor[i] = startPoint[i] -endPoint[i];
            }
            return vertor;
        }

        public static Point GetNormalizationVector(Point point)
        {
            var length = GetVectorLength(point);
            var result = new Point(point.Dimension());
            for (int i = 0; i < point.Dimension(); i++)
            {
                result[i] = point[i] / length;
            }

            return result;
        }

        public static double GetCosVectors(Point vector1, Point vector2)
        {
            var length1 = GetVectorLength(vector1);
            var length2 = GetVectorLength(vector2);
            var scalar = GetScalarProduct(vector1, vector2);

            return scalar / (length1 * length2);
        }
        public static double GetVectorLength(Point point)
        {
            double length = 0;
            for (int i = 0; i < point.Dimension(); i++)
            {
                length += point[i] * point[i];
            }

            return Math.Sqrt(length);
        }

        public static double GetScalarProduct(Point vector1, Point vector2)
        {
            double scalar = 0;
            for (int i = 0; i < vector1.Dimension(); i++)
            {
                scalar += vector1[i] * vector2[i];
            }

            return scalar;
        }


    }
}