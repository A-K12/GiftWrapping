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
            var dimention = points[0].Dimension();
            var mainNormal = new Point(dimention);
            mainNormal[0] = 1;
            var firstPoint = FindStartingPoint(points);
            facePoints.Add(firstPoint);
            pointsList.Remove(firstPoint);
            for (int i = 0; i < dimention-1; i++)
            {
                int index = 0;
                double maxValue = 0 ;
               
                var nextNormal  =new Point(dimention);
                for (int j = 0; j < pointsList.Count; j++)
                {
                    var vertexPoints = new List<IPoint>(facePoints);
                    vertexPoints.Add(pointsList[j]);
                    var matrix = MatrixHelper.CreateMatrix(vertexPoints.ToArray());
                    //normal = MatrixHelper.CalculateNormal(matrix);
                    var vector = new Point(dimention);
                    //nextNormal = LinearEquations.GaussWithChoiceSolveSystem(matrix, vector);
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