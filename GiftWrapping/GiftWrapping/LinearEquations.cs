using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using GiftWrapping.Structures;


namespace GiftWrapping
{
    public static class LinearEquations
    {
        static private double _eps = 1e-8;

        public static Vector GaussWithChoiceSolveSystem(Matrix matrix, Vector vector)
        {
            var row = new bool[matrix.Rows];
            var column = new bool[matrix.Cols];
            var dimension = matrix.Cols;
            var columnPosition = new int[dimension];
            var tempMatrix = (double[,])matrix;
            var tempVector = (double[]) vector;
            for (int i = 0; i < dimension; i++)
            {
                columnPosition[i] = i;
            }

            for (int i = 0; i < matrix.Rows; i++)
            {
                var majorColumn = i;
                var majorRow = i;
                for (int j = i; j < matrix.Rows; j++)
                    for (int k = 0; k < dimension; k++)
                        if (Math.Abs(tempMatrix[j,k]) > Math.Abs(tempMatrix[majorRow,majorColumn]))
                        {
                            majorColumn = k;
                            majorRow = j;
                        }

                Console.Out.WriteLine("Максимальный вектор = {0}", tempMatrix[majorRow,majorColumn]);
                Console.Out.WriteLine(" Индексы = Строка {0}, Колонка{1}", majorRow,majorColumn);

                SwapRows(ref tempMatrix, i, majorRow);
                SwapCoordinates(ref tempVector, i, majorRow);

                SwapColumn(ref tempMatrix, i, majorColumn);
                SwapCoordinates(ref columnPosition,i, majorColumn);

                Show(tempMatrix, tempVector, "После свайпа");
                var index = i;
                for (int j = i+1; j < matrix.Rows; j++)
                {
                    Console.Out.WriteLine("Line = {0}", j);
                    var multiplier = tempMatrix[j,i] / tempMatrix[index,i];
                    Console.Out.WriteLine("multiplier = {0}", multiplier);
                    tempMatrix[j,i] = 0;
                    for (int k = i+1; k < dimension; k++)
                    {
                        tempMatrix[j,k] -= multiplier * tempMatrix[index,k];
                    }
                    tempVector[j] -= multiplier * tempVector[index];
                    Show(tempMatrix, tempVector, "После умножения");
                }
            }
            var answer = new double[dimension];
            var indexLastX = Array.IndexOf(columnPosition, dimension - 1);
            answer[indexLastX] = 1.0;  
            for (int i = matrix.Rows-1; i >= 0; i--)
            {   
                var x = tempVector[i];
                for (int j = dimension - 1; j > i; --j)
                {
                    x -= tempMatrix[i,j] * answer[j];
                }
                if (Math.Abs(tempMatrix[i,i]) > _eps)
                {
                    x /= tempMatrix[i,i];
                }
                answer[i] = x;
            }
            var sortAnswer = new double[dimension];
            for (int i = 0; i < dimension; ++i)
            {
                var j = (int)columnPosition[i];
                sortAnswer[j] = answer[i];
            }

            return new Vector(sortAnswer);
            //return new Point(4);
        }


        private static void Show(double[,] matrix, double[] vector, string text = "Матрица")
        {
            Console.Out.WriteLine(text);
            Console.Out.WriteLine("---------------");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                
                for (int j = 0; j < vector.Length; j++)
                {
                    Console.Out.Write("{0:0.###} ", matrix[i,j]);
                }
                Console.Out.Write(" | "+ vector[i]);
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

        private static void SwapCoordinates(ref int[] tempMatrix, int indexA, int indexB)
        {
            var temp = tempMatrix[indexA];
            tempMatrix[indexA] = tempMatrix[indexB];
            tempMatrix[indexB] = temp;
        }
    }
}