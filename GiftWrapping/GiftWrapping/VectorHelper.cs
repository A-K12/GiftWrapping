using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class VectorHelper
    {

        public static bool HaveSameDimension(this IList<Vector> vectors)
        {
            return vectors.All(v => v.Dim == vectors[0].Dim);
        }

        public static Matrix ToMatrix(this IList<Vector> vectors)
        {
            if (vectors.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(vectors));
            }
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Points don't have same dimension");
            }
            int n = vectors.Count, m = vectors[0].Dim;
            double[] cells = new double[n * m];

            for (int i = 0; i < n; i++)
            {
                Array.Copy(vectors[i], 0, cells, m * i, m);
            }

            return new Matrix(n, m, cells.ToArray());
        }

        public static Matrix ToMatrix(this Vector vector)
        {
            return new Matrix(1, vector.Dim, vector);
        }

    }
}