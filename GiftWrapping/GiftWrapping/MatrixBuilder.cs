using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class MatrixBuilder
    {
       public static Matrix CreateMatrix(List<Point> points)
        {
            if (points.Count < 2)
            {
                throw new InvalidOperationException("Sequence contains less than two elements");
            }
            var dimension = points[0].Dim;
            if (points.Count > dimension)
            {
                throw new InvalidOperationException("Sequence contains more elements than point dimension");
            }
            var matrix = new double[dimension - 1, dimension];
            for (int i = 0; i < dimension-1; i++)
            {
                matrix[i, i + 1] = 1.0;
            }
            for (int i = 0; i < points.Count-1; i++)
            {
                var vector = Point.ToVector(points[i+1], points[0]);
                for (int j = 0; j < dimension; j++)
                {
                    matrix[i, j] = vector[j];
                }
            }

            return new Matrix(matrix);
        }
    }
}