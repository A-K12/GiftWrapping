using System;

namespace GiftWrapping.LinearEquations
{
    public class IndexMatrix
    {
        private IndexMap _rowIndexes, _colsIndexes;
        private readonly double[,] _matrix;

        public int Rows { get; }
        public int Cols { get; }

        public double this[Index i, Index j]
        {
            set => _matrix[_rowIndexes[i], _colsIndexes[j]] = value;
            get => _matrix[_rowIndexes[i], _colsIndexes[j]];
        }

        public IndexMatrix(double[,] matrix)
        {
            this._matrix = matrix;
            Rows= matrix.GetLength(0);
            Cols = matrix.GetLength(1);
            _rowIndexes = new IndexMap(Rows);
            _colsIndexes = new IndexMap(Cols);
        }

        public void SwapRows(Index index1, Index index2)
        {
            SwapCoordinates(ref _rowIndexes, index1, index2);
        }

        public void SwapColumns(Index index1, Index index2)
        {
            SwapCoordinates(ref _colsIndexes, index1, index2);
        }

        private void SwapCoordinates(ref IndexMap indexVector, Index index1, Index index2)
        {
            var temp = indexVector[index1];
            indexVector[index1] = indexVector[index2];
            indexVector[index2] = temp;
        }

        public (int, int) FindMaxAbsItem(int rowSubMatrix = 0, int colSubMatrix = 0)
        {
            var majorColumn = colSubMatrix;
            var majorRow = rowSubMatrix;
            for (int j = rowSubMatrix; j < Rows; j++)
            {
                for (int k = colSubMatrix; k < Cols; k++)
                {
                    if (Math.Abs(this[j, k]) >
                        Math.Abs(this[majorRow, majorColumn]))
                    {
                        majorColumn = k;
                        majorRow = j;
                    }
                }
            }

            return (majorRow, majorColumn);
        }
    }
}