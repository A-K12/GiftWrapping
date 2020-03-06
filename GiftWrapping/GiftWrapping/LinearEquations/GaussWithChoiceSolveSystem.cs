using System;
using GiftWrapping.Structures;
using GiftWrapping.LinearEquations;


namespace GiftWrapping.LinearEquations
{
    public class GaussWithChoiceSolveSystem
    {
        public Vector GetRandomAnswer(Matrix matrix, Vector vector)
        {
            var equations = new LinearEquations(matrix, vector); ;
            
            for (int i = 0; i < equations.Matrix.Rows; i++)
            {
                var coords = equations.Matrix.FindMaxAbsItem(i, i);
                equations.SwapRows(i, coords.Item1);
                equations.SwapColumns(i, coords.Item2);
                equations = DecreaseValuesOfEquations(equations, i);
            }
            if (matrix.Rows < matrix.Cols)
            {
                FillFreeVariables(equations);
            }

            return FindVariables(equations);
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
        private LinearEquations DecreaseValuesOfEquations(LinearEquations equations, int startSubMatrix)
        {
            var j = startSubMatrix;
            for (int i = j + 1; i < equations.Matrix.Rows; i++)
            {
                var multiplier = equations.Matrix[i, j] / equations.Matrix[j, j];
                equations.Matrix[i, j] = 0;

                for (int k = j+ 1; k < equations.Matrix.Cols; k++)
                {
                    equations.Matrix[i, k] -= multiplier * equations.Matrix[j, k];
                }

                equations.Vector[i] -= multiplier * equations.Vector[j];
            }

            return equations;
        }

        private void FillFreeVariables(LinearEquations equations)
        {
            var numberFreedomVariables = equations.Matrix.Cols - equations.Matrix.Rows;
            for (int i = 1; i < numberFreedomVariables + 1; i++)
            {
                equations.Variables[^i] = 1.0;
            }
        }

        private Vector FindVariables(LinearEquations equations)
        {
            for (int i = equations.Matrix.Rows - 1; i >= 0; i--)
            {
                var x = equations.Vector[i];

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