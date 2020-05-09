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
                throw new ArgumentException("Basis don't have same dimension");
            }
            if (points.Count != mask.Length)
            {
                throw new ArgumentException("Number of points is not equal to dimension.");
            }
            Vector[] vectors = points.ToVectors();
            Hyperplane hyperplane = Create(points.First(), vectors, mask);
            

            return hyperplane;
        }
        public static Hyperplane Create(Point point, IList<Vector> vectors, IndexMap mask)
        {
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Vectors don't have same dimension");
            }
            if (point.Dim != vectors[0].Dim)
            {
                throw new ArgumentException("Vectors and points have different dimensions.");
            }

            Matrix matrix = vectors.ToMatrix();

            Vector normal = ComputeNormal(matrix.TakeCols(mask._indexes));
            Hyperplane hyperplane = new Hyperplane(point, normal, mask);
            hyperplane.Basis = vectors.ToArray();
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