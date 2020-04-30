using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PlaneFinder
    {
        private PointIterator _pointIterator;

        private int _dim;

        private bool[] _indexMap;

        private Vector[] vectors;


        public Hyperplane FindFirstPlane(IList<Point> points)
        {
            this._pointIterator = new PointIterator(points);
            _dim = points[0].Dim;
            _indexMap = new bool[_dim - 1];
            Hyperplane mainPlane = GetStartPlane();
            vectors = GetFirstVectors();
            for (int i = 1; i < _dim; i++)
            {
                _pointIterator.ExcludePoint(mainPlane.MainPoint);
                Hyperplane maxPlane = GetMaxPlane(mainPlane);
                mainPlane = maxPlane;
                mainPlane.ReorientNormal();
            }

            return mainPlane;
        }
        private Hyperplane GetStartPlane()
        {
            Vector[] firstVectors = GetFirstVectors();
            Hyperplane hyperplane = HyperplaneHelper.Create(_pointIterator.MinimumPoint, firstVectors);
            hyperplane.ReorientNormal();

            return hyperplane;
        }
        private Hyperplane GetMaxPlane(Hyperplane mainPlane)    
        {
            double maxAngle = double.MinValue;
            Hyperplane maxPlane = mainPlane;
            Vector[] tempVectors = vectors;
            foreach (Point point in _pointIterator)
            {
                Vector vector = Point.ToVector(_pointIterator.MinimumPoint, point);
                tempVectors = SetVector(vector);
                Hyperplane plane = HyperplaneHelper.Create(_pointIterator.MinimumPoint, tempVectors);
                double angle = mainPlane.Angle(plane);
                if (angle < maxAngle) continue;
                maxAngle = angle;
                maxPlane = plane;
            }

            vectors = tempVectors;
            return maxPlane;
        }


        private Vector[] GetFirstVectors()
        {
            Vector[] vectors = new Vector[_dim-1];
            for (int i = 0; i < vectors.Length; i++)
            {
                double[] cells = new double[_dim];
                cells[i + 1] = 1;
                vectors[i] = new Vector(cells);
            }

            return vectors;
        }


        private Vector[] SetVector(Vector vector)
        {
            Vector[] newVectors = (Vector[]) vectors.Clone();
            if (vectors.Any(t => Vector.AreParallel(t, vector)))
            {
                return newVectors;
            }
            for (int i = 0; i < vectors.Length; i++)
            {
                if (_indexMap[i]) continue;
                newVectors[i] = vector;
                return newVectors;
            }

            throw new ArgumentException("Cannot insert a vector");
        }

    }
}