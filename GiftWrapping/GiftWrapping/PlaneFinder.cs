using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PlaneFinder
    {
        public Hyperplane FindFirstPlane(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            PlanePoint minPlanePoint = points.Min();
            Vector[] mainBasis = GetFirstBasis(dim);
            bool[] availableVectors = new bool[points.Count];
            Vector[] vectors = points.Select(point => Point.ToVector(minPlanePoint, point)).ToArray();
            Vector[] subBasis = mainBasis[1..^0];
            Vector mainVector = mainBasis[0];
            for (int i = 0; i < dim - 1; i++)
            {
                double minCos = double.MaxValue;
                int processedVector = default;
                Vector nextVector = default;
                for (int j = 0; j < vectors.Length; j++)
                {
                    if (availableVectors[j]) continue;
                    Vector ortVector = subBasis.GetOrthonormalVector(vectors[i]);
                    if (Tools.EQ(ortVector.Length)) continue;
                    double newCos = ortVector.Cos(mainVector);
                    if (Tools.GT(newCos, minCos)) continue;
                    nextVector = vectors[i];
                    processedVector = j;
                    minCos = newCos;
                }
                availableVectors[processedVector] = true;
                mainBasis[i] = nextVector;
                if(i==dim-2) continue;
                subBasis[i] = nextVector;
                mainVector = subBasis.GetOrthonormalVector(mainBasis[i+1]);
            }
            Hyperplane plane = HyperplaneBuilder.Create(minPlanePoint, mainBasis);
            plane.SetOrientationNormal(points);
            return plane;
        }

        private Vector[] GetFirstBasis(int dimension)
        {
            Vector[] vectors = new Vector[dimension - 1];
            for (int i = 0; i < vectors.Length; i++)
            {
                double[] cells = new double[dimension];
                cells[i + 1] = 1;
                vectors[i] = new Vector(cells);
            }

            return vectors;
        }

    }
}