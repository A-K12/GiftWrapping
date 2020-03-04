using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GiftWrapping.Structures;


namespace GiftWrapping
{
    public static class LinearEquationsSolver
    {
        static private double _eps = 1e-8;

        public static Vector GaussWithChoiceSolveSystem(Matrix matrix, Vector vector)
        {
            var equations = new LinearEquations(matrix, vector); ;
            var dim = matrix.Cols;
            
            for (int i = 0; i < equations.Rows; i++)
            {
                var majorColumn = i;
                var majorRow = i;
                for (int j = i; j < equations.Rows; j++)
                {
                    for (int k = 0; k < equations.Cols; k++)
                    {
                        if (Math.Abs(equations[j,k]) > 
                            Math.Abs(equations[majorRow,majorColumn]))
                        {
                            majorColumn = k;
                            majorRow = j;
                        }
                    }
                }
                Show(equations, "Начальная матрица");
                Console.Out.WriteLine("Максимальный вектор = {0}", equations[majorRow,majorColumn]);
                Console.Out.WriteLine(" Индексы = Строка {0}, Колонка{1}", majorRow,majorColumn);

                equations.SwapRows(i, majorRow);
                Show(equations, "После свайпа строк");
                
                equations.SwapColumns(i, majorColumn);
                Show(equations,"После свайпа столбцов");

                Show(equations, "После свайпа");
                var index =i;
                for (int j = i + 1; j < matrix.Rows; j++)
                {
                    Console.Out.WriteLine("Line = {0}", j);
                    var multiplier = equations[j,i] / equations[index,i];
                    Console.Out.WriteLine("multiplier = {0}", multiplier);
                    equations[j,i] = 0;
                    for (int k = i + 1; k < dim; k++)
                    {
                        equations[j,k] -= multiplier * equations[index,k];
                    }

                    equations[j] -= multiplier * equations[index];
                    Show(equations, "После умножения");
                    
                }
            }
            equations.SetVariable(dim-1, 1.0);
            for (int i = matrix.Rows-1; i >= 0; i--)
            {   
                var x = equations[i];
                for (int j = dim - 1; j > i; --j)
                {
                    x -= equations[i,j] * equations.GetVariable(j);
                }
                if (Math.Abs(equations[i,i]) > _eps)
                {
                    x /= equations[i,i];
                }
                equations.SetVariable(i,x);
            }
            
            return equations.GetVariables();
        }

      


        private static void Show(LinearEquations equations, string text)
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


        private static void SwapRows(ref double[,] matrix, int indexA, int indexB)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var temp = matrix[indexA, i];
                matrix[indexA, i] = matrix[indexB, i];
                matrix[indexB, i] = temp;
            }
            
        }

        private static void SwapColumn(ref double[,] matrix, int indexA, int indexB)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var temp = matrix[i,indexA];
                matrix[i, indexA] = matrix[i, indexB];
                matrix[i, indexB] = temp;
            }
        }

        private static void SwapCoordinates(ref double[] vector, int indexA, int indexB)
        {
            var temp = vector[indexA];
            vector[indexA] = vector[indexB];
            vector[indexB] = temp;
        }

        private static void SwapCoordinates(ref int[] vector, int indexA, int indexB)
        {
            var temp = vector[indexA];
            vector[indexA] = vector[indexB];
            vector[indexB] = temp;
        }
    }
}