using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping.Helpers
{
    public static class HyperplaneHelper
    {
        public static Hyperplane Create(IList<Point> points)
        {
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Points don't have same dimension");
            }
            if (points.Count != points[0].Dim)
            {
                throw new ArgumentException("Number of points is not equal to dimension.");
            }
            Vector[] vectors = points.ToVectors();
            Matrix matrix = vectors.ToMatrix();

            Hyperplane hyperplane = Create(points.Last(), matrix);

            return hyperplane;
        }
        public static Hyperplane Create(Point point, IList<Vector> vectors)
        {
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Vectors don't have same dimension");
            }
            if (point.Dim != vectors[0].Dim)
            {
                throw new ArgumentException("Vectors and points have different dimensions.");
            }

            Vector normal = ComputeNormal(vectors.ToMatrix());
            Hyperplane hyperplane = new Hyperplane(point, normal);

            return hyperplane;
        }
        public static Hyperplane Create(Point point, Matrix matrix)
        {
            if (point.Dim != matrix.Rows)
            {
                throw new ArgumentException("Vectors and points have different dimensions..");
            }
            if (point.Dim - 1 > matrix.Rows)
            {
                throw new ArgumentException("The plane cannot be found . There are not enough vectors.");
            }

            Vector normal = ComputeNormal(matrix);
            Hyperplane hyperplane = new Hyperplane(point, normal);

            return hyperplane;
        }

        private static Vector ComputeNormal(Matrix leftSide)
        {
            Vector rightSide = new Vector(leftSide.Rows);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
        }
    }
}