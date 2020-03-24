using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using Microsoft.VisualBasic;

namespace GiftWrapping
{
    public class PlaneFinder
    {
        public static Hyperplane FindFirstPlane(IList<Point> points)
        {
            if (points.Count == 0)
            {
                throw new ArgumentException("Sequence contains no elements");
            }
            int dim = points[0].Dim;
            Vector firstNormal = GetFirstNormal(dim); 
            int firstIndex = points.FindIndexMinimumPoint();
            int[] indexes = new int[dim];
            indexes[0] = firstIndex;
            Hyperplane mainPlane = new Hyperplane(points[firstIndex],firstNormal);
            List<Vector> planeVectors = new List<Vector>();
            for (int i = 1; i < dim; i++)
            {
                int index = 0;
                double maxAngle = double.MinValue;
                Hyperplane maxPlane = mainPlane;
                Vector maxVector = default;
                for (int j = 0; j < points.Count; j++)
                {
                     if(indexes.Contains(j)) continue;
                    Vector vector = Point.ToVector(points[firstIndex], points[j]);

                    Vector[] vectors = CreatePlaneVectors(new List<Vector>(planeVectors){vector});//really? 

                    Hyperplane nextPlane = Hyperplane.Create(points[j], vectors);

                    double angle = mainPlane.Angle(nextPlane);

                    if (angle > maxAngle)
                    {
                        maxAngle = angle;
                        index = j;
                        maxPlane= nextPlane;
                        maxVector = vector;
                    }
                }
                planeVectors.Add(maxVector);
                indexes[i] = index;
                maxPlane.TryAddPoints(mainPlane.Points);
                mainPlane = maxPlane;
            }

            return mainPlane;
        }

        private static Vector[] CreatePlaneVectors(IList<Vector> vectors)
        {
            int dim = vectors[0].Dim;
            Vector[] planeVectors = new Vector[dim - 1];
            for (int i = 0; i < vectors.Count; i++)
            {
                planeVectors[i] = vectors[i];
            }

            for (int i = vectors.Count; i < planeVectors.Length; i++)
            {
                double[] vector = new double[dim];
                vector[^i] = 1;
                planeVectors[i] = new Vector(vector);
            }

            return planeVectors;
        }

        private static Vector GetFirstNormal(int dimension)
        {
            double[] v = new double[dimension];
            v[0] = -1;
            return new Vector(v);
        }

    }
}