﻿using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PlaneFinder
    {
        private PointIterator _pointIterator;

        private int _dim;

        private bool[] _indexMap;

        public PlaneFinder(IList<Point> points)
        {
            
            if (points.Count == 0)
            {
                throw new ArgumentException("Sequence contains no elements");
            }
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Points don't have same dimension");
            }

            this._pointIterator = new PointIterator(points);
            _dim = points[0].Dim;
            _indexMap = new bool[_dim-1];
        }

        public Hyperplane FindFirstPlane()
        {
            Hyperplane mainPlane = GetStartPlane();
            for (int i = 1; i < _dim; i++)
            {
                _pointIterator.ExcludePoint(mainPlane.MainPoint);
                Hyperplane maxPlane = GetMaxPlane(mainPlane);
                maxPlane.TryAddPoints(mainPlane.Points);
                mainPlane = maxPlane;
                mainPlane.ReorientNormal();
            }

            return mainPlane;
        }
        private Hyperplane GetStartPlane()
        {
            Vector[] firstVectors = GetFirstVectors();
            Hyperplane hyperplane = Hyperplane.Create(_pointIterator.MinimumPoint, firstVectors);
            hyperplane.ReorientNormal();

            return hyperplane;
        }
        private Hyperplane GetMaxPlane(Hyperplane mainPlane)    
        {
            double maxAngle = double.MinValue;
            Hyperplane maxPlane = mainPlane;
            foreach (Point point in _pointIterator)
            {
                Vector vector = Point.ToVector(_pointIterator.MinimumPoint, point);
                IList<Vector> vectors = SetVector(mainPlane.BaseVectors, vector);
                Hyperplane plane = Hyperplane.Create(point, vectors);
                double angle = mainPlane.Angle(plane);

                if (angle < maxAngle) continue;
                maxAngle = angle;
                maxPlane = plane;
            }

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


        private IList<Vector> SetVector(ICollection<Vector> vectors, Vector vector)
        {
            List<Vector> newVectors = new List<Vector>(vectors);
            if (vectors.Any(t => Vector.AreParallel(t, vector)))
            {
                return newVectors;
            }
            for (int i = 0; i < vectors.Count; i++)
            {
                if (_indexMap[i]) continue;
                newVectors[i] = vector;
                return newVectors;
            }

            throw new ArgumentException("Cannot insert a vector");
        }

    }
}