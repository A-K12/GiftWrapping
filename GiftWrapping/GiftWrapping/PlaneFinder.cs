using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PlaneFinder
    {
        private int _dim;
        private bool[] _freeFieldsOfBasis;


        public Hyperplane FindFirstPlane(IList<PlanePoint> points)
        {
            _dim = points[0].Dim;
            _freeFieldsOfBasis = new bool[_dim - 1];
            PlanePoint minPlanePoint = points.Min();
            Hyperplane mainPlane = GetFirstHyperplane(minPlanePoint);
            bool[] availablePoints = new bool[points.Count];
            availablePoints[points.IndexOf(minPlanePoint)] = true;
            for (int i = 1; i < _dim; i++)
            {
                double maxAngle = double.MinValue;
                int maxPoint = default;
                Hyperplane maxPlane = mainPlane;
                for (int j = 0; j < points.Count; j++)
                {
                    if (availablePoints[j]) continue;
                    Vector vector = Point.ToVector(minPlanePoint, points[j]);
                    Vector[] tempBasis = SetVector(mainPlane.Basis, vector);
                    Hyperplane newPlane = HyperplaneHelper.Create(minPlanePoint, tempBasis);
                    double newAngle = mainPlane.Angle(newPlane);
                    if (Tools.LT(newAngle, maxAngle)) continue;
                    maxAngle = newAngle;
                    maxPlane = newPlane;
                    maxPoint = j;
                }

                availablePoints[maxPoint] = true;
                mainPlane = maxPlane;
                mainPlane.ReorientNormal();
            }

            return mainPlane;
        }

        private Hyperplane GetFirstHyperplane(Point point)
        {
            Vector normal = GetFirstNormal();
            Vector[] basis = GetFirstBasis();

            return  new Hyperplane(point, normal) {Basis = basis};
        }

        private Vector GetFirstNormal()
        {
            double[] normal = new double[_dim];
            normal[0] = -1;

            return new Vector(normal);
        }

        private Vector[] GetFirstBasis()
        {
            Vector[] vectors = new Vector[_dim - 1];
            for (int i = 0; i < vectors.Length; i++)
            {
                double[] cells = new double[_dim];
                cells[i + 1] = 1;
                vectors[i] = new Vector(cells);
            }

            return vectors;
        }

        private Vector[] SetVector(Vector[] vectors, Vector vector)
        {
            Vector[] newVectors = (Vector[]) vectors.Clone();
            if (vectors.Any(t => Vector.AreParallel(t, vector)))
            {
                return newVectors;
            }

            for (int i = 0; i < vectors.Length; i++)
            {
                if (_freeFieldsOfBasis[i]) continue;
                newVectors[i] = vector;
                return newVectors;
            }

            throw new ArgumentException("Cannot insert a vector");
        }


        private Vector FindNormal(Vector[] basis)
        {
            Matrix leftSide = basis.ToHorizontalMatrix();
            Vector rightSide = new Vector(leftSide.Rows);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
        }
    }
}