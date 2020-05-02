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
        private PointIterator _pointIterator;

        private int _dim;

        private bool[] _indexMap;

        private Vector[] _vectors;

        private IndexMap _mask;

        private Point _minimalPoint;

        public Hyperplane FindFirstPlane(IList<Point> points, IndexMap mask)
        {
            this._pointIterator = new PointIterator(points);
            this._mask = mask;
            _dim = mask.Length;
            _indexMap = new bool[_dim - 1];
            _minimalPoint = points.FindMinimumPoint(mask);
            Hyperplane mainPlane = GetStartPlane();
            _vectors = GetFirstVectors();
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
            Hyperplane hyperplane = HyperplaneHelper.Create(_minimalPoint, firstVectors, _mask);
            hyperplane.ReorientNormal();

            return hyperplane;
        }
        private Hyperplane GetMaxPlane(Hyperplane mainPlane)    
        {
            double maxAngle = double.MinValue;
            Hyperplane maxPlane = mainPlane;
            Vector[] tempVectors = _vectors;
            foreach (Point point in _pointIterator)
            {
                Vector vector = ConvertToVector(_minimalPoint, point);
                tempVectors = SetVector(vector);
                Hyperplane plane = HyperplaneHelper.Create(_minimalPoint, tempVectors, _mask);
                double angle = mainPlane.Angle(plane);
                if (angle < maxAngle) continue;
                maxAngle = angle;
                maxPlane = plane;
            }

            _vectors = tempVectors;
            return maxPlane;
        }

        private Vector ConvertToVector(Point begin, Point end)
        {
            double[] coordinates = new double[_mask.Length];
            for (int i = 0; i < _mask.Length; i++)
            {
                coordinates[i] = end[_mask[i]] - begin[_mask[i]];
            }

            return new Vector(coordinates);
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
            Vector[] newVectors = (Vector[]) _vectors.Clone();
            if (_vectors.Any(t => Vector.AreParallel(t, vector)))
            {
                return newVectors;
            }
            for (int i = 0; i < _vectors.Length; i++)
            {
                if (_indexMap[i]) continue;
                newVectors[i] = vector;
                return newVectors;
            }

            throw new ArgumentException("Cannot insert a vector");
        }

    }
}