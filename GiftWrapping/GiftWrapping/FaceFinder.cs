using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class FaceFinder
    {
        public IPoint[] FindFacePoints(IPoint[] points)
        {
            var facePoints =new List<IPoint>(); 
            var pointsList = new List<IPoint>(points);
            var dimention = points[0].GetDimension();
            var vector = new double[dimention];
            vector[0] = 1;
            var firstPoint = FindStartingPoint(points);
            facePoints.Add(firstPoint);
            pointsList.Remove(firstPoint);
            for (int i = 0; i < dimention-1; i++)
            {
                int index = 0;
                double maxValue = 0 ;
                double[] normal = new double[dimention];
                for (int j = 0; j < pointsList.Count; j++)
                {
                    var vertexPoints = new List<IPoint>(facePoints);
                    vertexPoints.Add(pointsList[j]);
                    var matrix = Matrix.CreateMatrix(vertexPoints.ToArray());
                    normal = Matrix.CalculateNormal(matrix);
                    var cosNormal = PointHelper.GetCosVectors(vector, normal);
                    if (cosNormal > maxValue)
                    {
                        maxValue = cosNormal;
                        index = j;
                    }
                }
                facePoints.Add(pointsList[index]);
                pointsList.RemoveAt(index);
                vector = normal;
            }

            return facePoints.ToArray();
        }

        public IPoint FindStartingPoint(IPoint[] points)
        {
            if (points.Length == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            return FindFirstPoint(points);
        }

        private IPoint FindFirstPoint(IPoint[] points)
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