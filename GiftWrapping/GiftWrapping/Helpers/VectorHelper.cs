using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping.Helpers
{
    public static class VectorHelper
    {

        public static bool HaveSameDimension(this IList<Vector> vectors)
        {
            return vectors.All(v => v.Dim == vectors[0].Dim);
        }

        public static Matrix ToHorizontalMatrix(this IList<Vector> vectors)
        {
            if (vectors.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(vectors));
            }
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Basis don't have same dimension");
            }
            int n = vectors.Count, m = vectors[0].Dim;
            double[] cells = new double[n * m];

            for (int i = 0; i < n; i++)
            {
                Array.Copy(vectors[i], 0, cells, m * i, m);
            }

            return new Matrix(n, m, cells.ToArray());
        }

        public static Matrix ToVerticalMatrix(this IList<Vector> vectors)
        {
            if (vectors.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(vectors));
            }
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Basis don't have same dimension");
            }
            int n = vectors.Count, m = vectors[0].Dim;
            double[] cells = new double[n * m];

            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    cells[n * j + i] = vectors[i][j];
                }
            }

            return new Matrix(m, n, cells.ToArray());
        }

        public static Matrix ToMatrix(this Vector vector)
        {
            return new Matrix(1, vector.Dim, vector);
        }

        public static Vector[] GetOrthonormalBasis(this IEnumerable<Vector> vectors)
        {
            //TODO Checking vectors

            Vector[] basis = vectors.Select((vector => new Vector(vector))).ToArray();
            for (int i = 1; i < basis.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    basis[i] -= basis[i].ProjectVectorTo(basis[j]);
                }

                basis[i] /= basis[i].Length;
            }

            return basis;
        }

        public static Vector ProjectVectorTo(this Vector v, Vector vector)
        {
            double coefficient = v * vector;
            coefficient /= vector.Length * vector.Length;

            return vector * coefficient;
        }
    }
}