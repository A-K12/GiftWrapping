using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;
using GiftWrapping.LinearEquations;


namespace GiftWrapping.LinearEquations
{
    public class GaussWithChoiceSolveSystem
    {
        public static Vector FindAnswer(Matrix leftSide, Vector rightSide )
        {
            Matrix matrix = new Matrix(leftSide);
            if (leftSide.Rows > leftSide.Cols)
            {
                matrix = ConvertToSquareMatrix(matrix);
            }
            LinearEquations equations = new LinearEquations(matrix, rightSide);
            for (int i = 0; i < equations.Matrix.Rows; i++)
            {
                (int row, int col)  = equations.Matrix.FindIndexMaxAbsItem(i, i);
                equations.SwapRows(i, row);
                equations.SwapColumns(i, col);
                equations = ExcludeMaximumElement(equations, i);
            }
            if (equations.Matrix.Rows < equations.Matrix.Cols)
            {
                FillFreeVariables(equations);
            }

            return FindVariables(equations);
        }

        private static Matrix ConvertToSquareMatrix(Matrix matrix)
        {
            int cols = matrix.Cols;
            int rows = matrix.Rows;
            int[] range = Enumerable.Range(0, cols).ToArray();
            double[,] squareMatrix = matrix.TakeRows(range);
            double[] sum = new double[cols];
            for (int i = cols; i < matrix.Rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum[j] += matrix[i, j];
                }
            }
            for (int i = 0; i < squareMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < squareMatrix.GetLength(1); j++)
                {
                    squareMatrix[i, j] += sum[j];
                }
            }

            return new Matrix(squareMatrix);
        }

        private void Show(LinearEquations equations, string text)
        {
            Console.Out.WriteLine(text);
            Console.Out.WriteLine("---------------");
            for (int i = 0; i < equations.Matrix.Rows; i++)
            {

                for (int j = 0; j < equations.Matrix.Cols; j++) 
                {
                    Console.Out.Write("{0:0.###} ", equations.Matrix[i, j]);
                }
                Console.Out.Write(" | " + equations.Vector[i]);
                Console.Out.WriteLine("");
            }
            Console.Out.WriteLine("_______________________");
            for (int j = 0; j < equations.Matrix.Cols; j++)
            {
                Console.Out.Write("{0:0.###} ", equations.Variables[j]);
            }
        }


        private static LinearEquations ExcludeMaximumElement(LinearEquations equations, int startSubMatrix)
        {
            int j = startSubMatrix;
            for (int i = j + 1; i < equations.Matrix.Rows; i++)
            {
                double multiplier = equations.Matrix[i, j] / equations.Matrix[j, j];
                equations.Matrix[i, j] = 0;

                for (int k = j+ 1; k < equations.Matrix.Cols; k++)
                {
                    equations.Matrix[i, k] -= multiplier * equations.Matrix[j, k];
                }

                equations.Vector[i] -= multiplier * equations.Vector[j];
            }

            return equations;
        }

        private static void FillFreeVariables(LinearEquations equations)
        {
            int numberFreedomVariables = equations.Matrix.Cols - equations.Matrix.Rows;
            for (int i = 1; i < numberFreedomVariables + 1; i++)
            {
                equations.Variables[^i] = 1.0;
            }
        }

        private static Vector FindVariables(LinearEquations equations)
        {
            for (int i = equations.Matrix.Rows - 1; i >= 0; i--)
            {
                double x = equations.Vector[i];

                for (int j = equations.Matrix.Cols-1; j > i; --j)
                {
                    x -= equations.Matrix[i, j] * equations.Variables[j];
                }

                if (Tools.NE(Math.Abs(equations.Matrix[i, i])))
                {
                    x /= equations.Matrix[i, i];
                }
                else
                {
                    throw new InvalidOperationException();
                }

                equations.Variables[i] = x;
            }

            return equations.Variables;
        }
    }
}