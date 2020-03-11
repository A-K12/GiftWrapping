using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class FaceFinder
    {
        private readonly GaussWithChoiceSolveSystem _gauss;

        public FaceFinder()
        {
            _gauss = new GaussWithChoiceSolveSystem();
        }

        public Point[] FindFacePoints(Point[] points)
        {
            if (points.Length == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }
            var facePoints = new List<Point>();
            var pointsList = new List<Point>(points);
            var dim = points[0].Dim;
            var mainNormal = GetFirstNormal(dim);
            var firstPoint = FindStartingPoint(points);
            facePoints.Add(firstPoint);
            pointsList.Remove(firstPoint);
            for (int i = 0; i < dim - 1; i++)
            {
                int index = 0;
                double maxValue = 0;
                var nextNormal = new Vector(dim);
                for (int j = 0; j < pointsList.Count; j++)
                {
                    var vertexPoints = new List<Point>(facePoints);
                    vertexPoints.Add(pointsList[j]);
                    var matrix = MatrixBuilder.CreateMatrix(vertexPoints);
                    var vector = new Vector(dim);
                    nextNormal = _gauss.FindAnswer(matrix, vector);
                    var cosNormal = Vector.Angle(nextNormal, mainNormal);
                    if (cosNormal > maxValue)
                    {
                        maxValue = cosNormal;
                        index = j;
                    }
                }
                facePoints.Add(pointsList[index]);
                pointsList.RemoveAt(index);
                mainNormal = nextNormal;
            }

            return facePoints.ToArray();
        }

        private Vector GetFirstNormal(int dimension)
        {
            var v = new double[dimension];
            v[0] = 1;//-1
            return new Vector(v);
        }

        public Point FindStartingPoint(Point[] points)
        {
            if (points.Length == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            var startPoint = points[0];
            foreach (var point in points)
            {
                if (point[0] < startPoint[0])
                {
                    startPoint = point;
                }
            }

            return startPoint;
        }

    }
}