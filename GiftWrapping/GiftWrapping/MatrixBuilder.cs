using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class MatrixBuilder
    {
       public static Matrix CreateMatrix(Point[] points)
        {
            if (points.Length < 2)
            {
                throw new InvalidOperationException("Sequence contains less than two elements");
            }
            int dimension = points[0].Dim;
            if (points.Length > dimension)   
            {
                throw new InvalidOperationException("Sequence contains more elements than point dimension");
            }
            double[,] matrix = new double[dimension - 1, dimension];
            for (int i = 0; i < dimension-1; i++)
            {
                matrix[i, i + 1] = 1.0;
            }
            for (int i = 0; i < points.Length-1; i++)
            {
                Vector vector = Point.ToVector(points[i+1], points[0]);
                for (int j = 0; j < dimension; j++)
                {
                    matrix[i, j] = vector[j];
                }
            }

            return new Matrix(matrix);
       }

       public static Matrix CreateMatrix(Vector[] vectors)
       {
           int dimension = vectors[0].Dim;
           if (vectors.Length > dimension)
           {
               throw new InvalidOperationException("Sequence contains more elements than point dimension");
           }
           double[,] matrix = new double[dimension - 1, dimension];
           for (int i = 0; i < dimension - 1; i++)
           {
               matrix[i, i + 1] = 1.0;
           }
           for (int i = 0; i < vectors.Length - 1; i++)
           {
               for (int j = 0; j < dimension; j++)
               {
                   matrix[i, j] = vectors[i][j];
               }
           }
      
           return new Matrix(matrix);
       }

    }
}