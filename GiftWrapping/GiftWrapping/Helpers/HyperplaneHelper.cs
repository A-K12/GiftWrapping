using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping.Helpers
{
    public static class HyperplaneHelper
    {
        public static Hyperplane Create(IList<Point> points, IndexMap mask)
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

            Hyperplane hyperplane = Create(points.Last(), matrix, mask);

            return hyperplane;
        }
        public static Hyperplane Create(Point point, IList<Vector> vectors, IndexMap mask)
        {
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Vectors don't have same dimension");
            }
            if (mask.Length != vectors[0].Dim)
            {
                throw new ArgumentException("Vectors and points have different dimensions.");
            }

            Vector normal = ComputeNormal(vectors.ToMatrix());
            Hyperplane hyperplane = new Hyperplane(point, normal, mask);

            return hyperplane;
        }
        public static Hyperplane Create(Point point, Matrix matrix, IndexMap mask)
        {
            if (mask.Length != matrix.Rows)
            {
                throw new ArgumentException("Vectors and points have different dimensions..");
            }
            if (mask.Length - 1 > matrix.Rows)
            {
                throw new ArgumentException("The plane cannot be found . There are not enough vectors.");
            }

            Vector normal = ComputeNormal(matrix);
            Hyperplane hyperplane = new Hyperplane(point, normal, mask);

            return hyperplane;
        }

        private static Vector ComputeNormal(Matrix leftSide)
        {
            Vector rightSide = new Vector(leftSide.Rows);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
        }

        public static IList<Point> GetPlanePoints(this Hyperplane h, IList<Point> points)
        {
            List<Point> result = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (h.IsPointInPlane(points[i]))
                {
                    result.Add(points[i]);
                }
            }

            return result;
        }
    }
}