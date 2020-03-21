using System;
using System.Collections.Generic;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PlaneFinder
    {
        private List<Point> planePoints, points;
        private int dim;

        public PlaneFinder(List<Point> points)
        {
            if (points.Count == 0)
            {
                throw new ArgumentException("Sequence contains no elements");
            }
            this.points = points;
            planePoints = new List<Point>();
            dim = points[0].Dim;
        }

        public Hyperplane FindFirstPlane()
        {
            Vector firstNormal = GetFirstNormal(dim);
            Point firstPoint = FindStartingPoint(points);
            Hyperplane mainPlane = new Hyperplane(firstPoint,firstNormal);
            planePoints.Add(firstPoint);
            points.Remove(firstPoint);
            for (int i = 0; i < dim - 1; i++)
            {
                int index = 0;
                double maxValue = double.MinValue;
                Hyperplane maxPlane = mainPlane;
                for (int j = 0; j < this.points.Count; j++)
                {
                    List<Point> vertexPoints = new List<Point>(planePoints)
                    {
                        this.points[j]
                    }; 
                    Vector normal = FindNormal(vertexPoints.ToArray());
                    Hyperplane nextPlane = new Hyperplane(vertexPoints[0], normal);
                    double angle = mainPlane.Angle(nextPlane);
                    if (angle > maxValue)
                    {
                        maxValue = angle;
                        index = j;
                        maxPlane= nextPlane;
                    }
                }
                planePoints.Add(this.points[index]);
                this.points.RemoveAt(index);
                mainPlane = maxPlane;
                maxPlane.TryAddPoints(planePoints.ToArray());
            }

            return mainPlane;
        }

        private Vector FindNormal(Point[] points)
        {
            Matrix leftSide = MatrixBuilder.CreateMatrix(points);
            Vector rightSide = new Vector(points[0].Dim);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide).Normalize();
        }
        private Vector GetFirstNormal(int dimension)
        {
            double[] v = new double[dimension];
            v[0] = -1;
            return new Vector(v);
        }

        public static Point FindStartingPoint(List<Point> points)
        {
            if (points.Count == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            Point startPoint = points[0];
            foreach (Point point in points)
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