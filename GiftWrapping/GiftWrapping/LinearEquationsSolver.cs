using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GiftWrapping.Structures;


namespace GiftWrapping
{
    public class LinearEquationsSolver
    {
        static private double _eps = 1e-8;

        public Vector GaussWithChoiceSolveSystem(Matrix matrix, Vector vector)
        {
            var equations = new LinearEquations(matrix, vector); ;
            
            for (int i = 0; i < equations.Rows; i++)
            {
                var majorCoord = equations.FindCoordinatesMaxItem(i, i);
                Show(equations, "Начальная матрица");
                Console.Out.WriteLine("Максимальный вектор = {0}", equations[majorCoord.Item1,majorCoord.Item2]);
                equations.SwapRows(i, majorCoord.Item1);
                equations.SwapColumns(i, majorCoord.Item2);
                Show(equations,"После свайпа");
                equations = ChangeMatrix(equations, i);
            }
            equations.SetVariable(equations.Cols-1, 1.0);

            return FindVariables(equations);
        }


        private LinearEquations ChangeMatrix(LinearEquations equations,int startSubMatrix)
        {
            for (int j = startSubMatrix + 1; j < equations.Rows; j++)
            {
                var multiplier = equations[j, startSubMatrix] / equations[startSubMatrix, startSubMatrix];
                equations[j, startSubMatrix] = 0;
                for (int k = startSubMatrix+ 1; k < equations.Cols; k++)
                {
                    equations[j, k] -= multiplier * equations[startSubMatrix, k];
                }

                equations[j] -= multiplier * equations[startSubMatrix];
            }

            return equations;
        }
        private Vector FindVariables(LinearEquations equations)
        {
            for (int i = equations.Rows - 1; i >= 0; i--)
            {
                var x = equations[i];
                for (int j = equations.Cols-1; j > i; --j)
                {
                    x -= equations[i, j] * equations.GetVariable(j);
                }
                if (Math.Abs(equations[i, i]) > _eps)
                {
                    x /= equations[i, i];
                }
                equations.SetVariable(i, x);
            }

            return equations.GetVariables();
        }

        private void Show(LinearEquations equations, string text)
        {
            Console.Out.WriteLine(text);
            Console.Out.WriteLine("---------------");
            for (int i = 0; i < equations.Rows; i++)
            {
                
                for (int j = 0; j < equations.Cols; j++)
                {
                    Console.Out.Write("{0:0.###} ", equations[i,j]);
                }
                Console.Out.Write(" | "+ equations[i]);
                Console.Out.WriteLine("");
            }
        }

    }
}