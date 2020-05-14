using System;
using GiftWrapping.Structures;

namespace GiftWrapping.Helpers
{
    public static class MatrixHelper
    {
        public static Vector[] ToColumnVectors(this Matrix matrix)
        {
            Vector[] vectors = new Vector[matrix.Cols];

            double[] cells = new double[matrix.Rows];
            for (int i = 0; i < matrix.Cols; i++)
            {
                for (int j = 0; j < matrix.Rows; j++)
                {
                    cells[i] = matrix[j, i];
                }
                vectors[i] = new Vector(cells);
            }

            return vectors;
        }

        public static Vector[] ToRowVectors(this Matrix matrix)
        {
            Vector[] vectors = new Vector[matrix.Rows];

            double[] cells = new double[matrix.Cols];
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    cells[i] = matrix[i, j];
                }
                vectors[i] = new Vector(cells);
            }

            return vectors;
        }

        public static double GetDeterminant(double[,] matrix)
        {
            if (matrix.Length == 2)
                return matrix[0,0] * matrix[1,1] - matrix[0,1] * matrix[1,0];
            int minorSing = 1;
            double result = 0;
            int col = 0;
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                minorSing = ((row + 1) % 2 == (col + 1) % 2) ? 1 : -1;
                result += minorSing * matrix[row,col] * GetDeterminant(GetMinorMatrix(matrix, row, col));
            }
            return result;
        }

        static public double[,] GetMinorMatrix(double[,] matrix, int row, int col)
        {
            var minorN = matrix.GetLength(0) - 1;
            var minorM = matrix.GetLength(1) - 1;
            double[,] result = new double[minorM, minorN];
            int m = 0, k;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i == row) continue;
                k = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == col) continue;
                    result[m,k++] = matrix[i,j];
                }
                m++;
            }
            var minor = result;
            return result;
        }


    }


}