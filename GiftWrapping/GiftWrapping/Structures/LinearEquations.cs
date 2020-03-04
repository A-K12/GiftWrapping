using System;

namespace GiftWrapping.Structures
{
    public class LinearEquations
    {
        private int[] rowIndexes, colsIndexes;
        private double[,] matrix;
        private double[] vector;
        private double[] variables;

        /// <summary>
        /// Number of rows of the matrix
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Number of columns of the matrix
        /// </summary>
        public int Cols { get; }

        public LinearEquations(Matrix leftSide, Vector rightSide)
        {
            matrix = leftSide;
            vector = rightSide;
            Rows = leftSide.Cols;
            Cols = leftSide.Cols;
            variables = new double[Rows];
            rowIndexes = GetIndexArray(leftSide.Rows);
            colsIndexes = GetIndexArray(leftSide.Cols);
        }

        private int[] GetIndexArray(int lenght)
        {
            var array = new int[lenght];
            for (int i = 0; i < lenght; i++)
            {
                array[i] = i;
            }

            return array;
        }

        public double this[int i]
        {
            set => vector[rowIndexes[i]] = value;
            get => vector[rowIndexes[i]];
        }
        public double this[int i, int j]
        {
            set => matrix[rowIndexes[i], colsIndexes[j]] = value;
            get => matrix[rowIndexes[i], colsIndexes[j]];
        }

        public void SetVariable(int index, double value)
        {
            variables[rowIndexes[index]] = value;
        }

        public double GetVariable(int index)
        {
            return variables[rowIndexes[index]];
        }

        public Vector GetVariables()
        {
            var sortVar = new double[Rows];
            for (int i = 0; i < Rows; ++i)
                sortVar[i] = variables[colsIndexes[i]];

            return new Vector(sortVar);
        }

        public void SwapRows(int index1, int index2)
        {
            SwapCoordinates(ref rowIndexes, index1, index2);
        }

        public void SwapColumns(int index1, int index2)
        {
            SwapCoordinates(ref colsIndexes, index1, index2);
        }
        private void SwapCoordinates(ref int[] vector, int index1, int index2)
        {
            var temp = vector[index1];
            vector[index1] = vector[index2];
            vector[index2] = temp;
        }
    }
}