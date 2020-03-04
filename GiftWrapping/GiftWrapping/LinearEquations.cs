using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GiftWrapping.Structures;


namespace GiftWrapping
{
    public static class LinearEquations
    {
        static private double _eps = 1e-8;

        public static Vector GaussWithChoiceSolveSystem(Matrix matrix, Vector vector)
        {
            var row = GetIndexArray(matrix.Rows);
            var column = GetIndexArray(matrix.Cols);
            var dim = matrix.Cols;
            var tempMatrix = (double[,])matrix;
            var tempVector = (double[]) vector;
           

            for (int i = 0; i < row.Length; i++)
            {
                var majorColumn = i;
                var majorRow = i;
                for (int j = i; j < row.Length; j++)
                {
                    for (int k = 0; k < column.Length; k++)
                    {
                        if (Math.Abs(tempMatrix[row[j], column[k]]) > 
                            Math.Abs(tempMatrix[row[majorRow], column[majorColumn]]))
                        {
                            majorColumn = k;
                            majorRow = j;
                        }
                    }
                }
                Show(tempMatrix, tempVector, "Начальная матрица", row, column);
                Console.Out.WriteLine("Максимальный вектор = {0}", tempMatrix[row[majorRow],column[majorColumn]]);
                Console.Out.WriteLine(" Индексы = Строка {0}, Колонка{1}", majorRow,majorColumn);


               
                SwapCoordinates(ref row, i, majorRow);
                Show(tempMatrix, tempVector, "После свайпа строк", row, column);
                
                SwapCoordinates(ref column,i, majorColumn);
                Show(tempMatrix, tempVector, "После свайпа столбцов", row, column);

                Show(tempMatrix, tempVector, "После свайпа", row, column);
                var index =i;
                for (int j = i + 1; j < matrix.Rows; j++)
                {
                    Console.Out.WriteLine("Line = {0}", j);
                    var multiplier = tempMatrix[row[j], column[i]] / tempMatrix[row[index],column[i]];
                    Console.Out.WriteLine("multiplier = {0}", multiplier);
                    tempMatrix[row[j], column[i]] = 0;
                    for (int k = i + 1; k < dim; k++)
                    {
                        tempMatrix[row[j], column[k]] -= multiplier * tempMatrix[row[index],column[k]];
                    }

                    tempVector[row[j]] -= multiplier * tempVector[row[index]];
                    Show(tempMatrix, tempVector, "После умножения", row, column);
                    
                }
            }
            var answer = new double[dim];
            var indexLastX = Array.IndexOf(column, dim - 1);
            answer[indexLastX] = 1.0;  
            for (int i = matrix.Rows-1; i >= 0; i--)
            {   
                var x = tempVector[row[i]];
                for (int j = dim - 1; j > i; --j)
                {
                    x -= tempMatrix[row[i],column[j]] * answer[j];
                }
                if (Math.Abs(tempMatrix[row[i],column[i]]) > _eps)
                {
                    x /= tempMatrix[row[i],column[i]];
                }
                answer[i] = x;
            }
            var sortAnswer = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                var j = (int)column[i];
                sortAnswer[j] = answer[i];
            }

            return new Vector(sortAnswer);
            //return new Point(4);
        }

        private static int[] GetIndexArray(int lenght)
        {
            var array = new int[lenght];
            for (int i = 0; i < lenght; i++)
            {
                array[i] = i;
            }

            return array;
        }


        private static void Show(double[,] matrix, double[] vector, string text, int[] row, int[] column)
        {
            Console.Out.WriteLine(text);
            Console.Out.WriteLine("---------------");
            for (int i = 0; i < row.Length; i++)
            {
                
                for (int j = 0; j < column.Length; j++)
                {
                    Console.Out.Write("{0:0.###} ", matrix[row[i],column[j]]);
                }
                Console.Out.Write(" | "+ vector[row[i]]);
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