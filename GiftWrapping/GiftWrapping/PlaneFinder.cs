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


        public Hyperplane FindFirstPlane(IList<Point> points)
        {
            _dim = points[0].Dim;
            _freeFieldsOfBasis = new bool[_dim - 1];
            Point minPoint = points.Min();
            Vector[] mainBasis = GetFirstBasis();
            Vector mainNormal = GetFirstNormal();
            bool[] availablePoints = new bool[points.Count];
            availablePoints[points.IndexOf(minPoint)] = true;
            for (int i = 1; i < _dim; i++)
            {
                double maxAngle = double.MinValue;
                int maxPoint = default;
                Vector[] maxBasis = default;
                Vector maxNormal = default;
                for (int j = 0; j < points.Count; j++)
                {
                    if (availablePoints[j]) continue;
                    Vector vector = Point.ToVector(minPoint, points[j]);
                    Vector[] tempBasis = SetVector(mainBasis, vector);
                    Vector newNormal = FindNormal(tempBasis);
                    double newAngle = Vector.Angle(mainNormal, newNormal);
                    if (Tools.LT(newAngle, maxAngle)) continue;
                    maxAngle = newAngle;
                    maxNormal = newNormal;
                    maxBasis = tempBasis;
                    maxPoint = j;
                }

                availablePoints[maxPoint] = true;
                mainBasis = maxBasis;
                mainNormal = -maxNormal;
            }

            IEnumerable<Point> planePoints = points.Where(((_, i) => availablePoints[i]));
            return HyperplaneHelper.Create(planePoints.ToList());
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
            Matrix leftSide = basis.ToMatrix();
            Vector rightSide = new Vector(leftSide.Rows);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
        }
    }
}