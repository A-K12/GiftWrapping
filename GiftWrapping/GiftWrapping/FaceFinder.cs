using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class FaceFinder
    {
        public Vector[] FindFacePoints(Vector[] points)
        {
            var facePoints = new List<Vector>();
            var pointsList = new List<Vector>(points);
            var dim = points[0].Dim;
            var mainNormal = new Vector(dim);
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
                    var vertexPoints = new List<Vector>(facePoints);
                    vertexPoints.Add(pointsList[j]);
                    var matrix = MatrixHelper.CreateMatrix(vertexPoints.ToArray());
                    //normal = MatrixHelper.CalculateNormal(matrix);
                    var vector = new Point(dim);
                    //nextNormal = GaussWithChoiceSolveSystem.GetRandomAnswer(matrix, vector);
                    var cosNormal = VectorHelper.GetCosVectors(nextNormal, mainNormal);
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

        public Vector FindStartingPoint(Vector[] points)
        {
            if (points.Length == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            return FindFirstPoint(points);
        }

        private Vector FindFirstPoint(Vector[] points)
        {
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