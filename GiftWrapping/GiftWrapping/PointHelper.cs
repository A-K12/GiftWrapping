using System;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class PointHelper
    {
        public static double[] CreateVertex(IPoint startPoint, IPoint endPoint)
        {
            var vertex = new double[startPoint.GetDimension()];
            for (int i = 0; i < startPoint.GetDimension(); i++)
            {
                vertex[i] = startPoint[i] -endPoint[i];
            }
            return vertex;
        }

        public static double[] GetNormalizationVector(double[] vector)
        {
            var length = GetVectorLength(vector);

            return vector.Select((cord => cord / length)).ToArray();
        }

        public static double GetCosVectors(double[] vector1, double[] vector2)
        {
            var length1 = GetVectorLength(vector1);
            var length2 = GetVectorLength(vector2);
            var scalar = GetScalarProduct(vector1, vector2);

            return scalar / (length1 * length2);
        }
        public static double GetVectorLength(double[] vector)
        {
            double length = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                length += vector[i] * vector[i];
            }

            return Math.Sqrt(length);
        }

        public static double GetScalarProduct(double[] vector1, double[] vector2)
        {
            double scalar = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                scalar += vector1[i] * vector2[i];
            }

            return scalar;
        }


    }
}