using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class MatrixHelper
    {
        //public static double[] CalculateNormal(double[][] matrix)
        //{
        //    var dimension = matrix[0].Length;
        //    var normal = new double[dimension];
        //    for (int i = 0; i <dimension; i++)
        //    {
        //        var minor = GetMinorMatrix(matrix, 0, i);
        //        normal[i] = GetDeterminant(minor);
        //    }

        //    return PointHelper.GetNormalizationVector(normal);
        //}



        public static Point[] CreateMatrix(IPoint[] points)
        {
            if (points.Length < 2)
            {
                throw new InvalidOperationException("Sequence contains less than two elements");
            }

            var dimension = points[0].Dimension();
            var vectors = new List<Point>();
            for (int i = 1; i < points.Length; i++)
            {
                var vector = VectorHelper.CreateVector(points[i], points[0]);
                vectors.Add(vector);
            }

            var matrix = new Point[dimension -1];

            for (int i = 0; i < vectors.Count; i++)
            {
                matrix[i] = vectors[i];
            }

            int index = dimension-1;
            for (int i = matrix.Length-1; i > vectors.Count -1 ; i--)
            {
                matrix[i] = new Point(dimension);
                matrix[i][index--] = 1.0;
            }
            return matrix;
        }

        public static double GetDeterminant(double[][] matrix)
        {
            if (matrix.Length == 2)
                return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            int minorSing = 1;
            double result = 0;
            int col = 0;
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                minorSing = ((row + 1) % 2 == (col + 1) % 2) ? 1 : -1;
                result += minorSing * matrix[row][col] * GetDeterminant(GetMinorMatrix(matrix, row, col));
            }
            return result;
        }

        static public double[][] GetMinorMatrix(double[][] matrix, int row, int col)
        {
            var minorN = matrix.GetLength(0) - 1;
            var minorM = matrix[0].Length-1;
            double[][] result = new double[minorM][];
            int m = 0, k;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i == row) continue;
                k = 0;
                result[m]=new double[minorN];
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    if (j == col) continue;
                    result[m][k++] = matrix[i][j];
                }
                m++;
            }
            var minor = result;
            return result;
        }
    }
}