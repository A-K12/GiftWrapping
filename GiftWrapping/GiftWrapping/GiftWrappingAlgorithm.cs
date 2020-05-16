﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Transactions;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class GiftWrappingAlgorithm
    {
        private double tolerance;
        public GiftWrappingAlgorithm(IList<Point> points, double tolerance)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("The number of _points must be more than three.");
            }
            _cell = new ConvexHull(points[0].Dim);
            this._points = points;
        }

        private IList<Point> _points;

        private ICell _cell;

        public ConvexHull Create(List<Point> points)
        {
            PlanePoint[] planePoints = points.Select((point => new PlanePoint(point, point))).ToArray();
            return FindConvexHull3D(planePoints);
        }

        protected void FindFirstFace(List<PlanePoint> points)
        {
            FindFirstFace(points);
        }
        protected ICell FindFirstFace(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            ConvexHull convexHull = new ConvexHull(points[0].Dim);
            Hyperplane hyperplane = FindFirstPlane(points);
            IList<PlanePoint> planePoints = hyperplane.GetPlanePoints(points);
            if (planePoints.Count == dim)
            {
                return CreateSimplex(planePoints);
            }
            if (dim - 1 == 3)
            {
                return FindConvexHull3D(points);
            }
            PlanePoint[] newPoints = planePoints.Select((point => hyperplane.ConvertPoint(point))).ToArray();
            ICell currentFace = FindFirstFace(newPoints);
            convexHull.InnerFaces.Add(currentFace);
            Queue<ICell> unprocessedEdges = new Queue<ICell>();
            unprocessedEdges.Enqueue(currentFace.);
            while (unprocessedEdges.Count!=0)
            {
                ICell edge = unprocessedEdges.Pop();
                Point[] edgePoints = new Point[edge.Hyperplane.Dimension+1];
                for (int i = 0; i < edgePoints.Length; i++)
                {
                   // edgePoints[i] = edge.Hyperplane.Basis[i]; Вместо точек нужно брать вектора
                }
                double maxAngle = double.MinValue;
                Hyperplane maxHyperplane = currentFace.Hyperplane;
                for (int i = 0; i < points.Count; i++)
                {
                    if(planePoints.Contains(points[i])) continue;
                    edgePoints[^1] = points[i];
                    IndexMap faceMap = GetIndexMap(edgePoints);//REMOVE
                    Hyperplane newHyperplane = HyperplaneHelper.Create(edgePoints);
                    double angle = currentFace.Hyperplane.Angle(newHyperplane);
                    if (angle < maxAngle) continue; //if ==? 
                    maxAngle = angle;
                    maxHyperplane = newHyperplane;
                }

                if (convexHull.InnerFaces.Any((face => face.Hyperplane.Equals(maxHyperplane)))) continue;

                IList<Point> facePoints = maxHyperplane.GetPlanePoints(points);

                ConvexHull convexConvexHull = FindConvexHull(facePoints);
                convexHull.InnerFaces.Add(convexConvexHull);
                foreach (var face in convexConvexHull.InnerFaces)
                {
                    unprocessedEdges.Push(face);
                }
            }

            return convexHull;
        }

        private ConvexHull FindConvexHull3D(IList<PlanePoint> points)
        {
            ConvexHull convexHull = new ConvexHull(3);

            Hyperplane currentHyperplane = FindFirstPlane(points);
            IList<PlanePoint> planePoints = currentHyperplane.GetPlanePoints(points);
            IList<PlanePoint> planePoints2d = planePoints.Select((point => currentHyperplane.ConvertPoint(point))).ToList(); 
            ConvexHull2d firstHull = FindConvexHull2D(planePoints2d);
            Queue<ICell> unprocessed = new Queue<ICell>();
            HashSet<Edge2d> processedEdges = new HashSet<Edge2d>();
            unprocessed.Enqueue(firstHull);
            Edge2d tempEdge = new Edge2d();
            while(unprocessed.Any())
            {
                ConvexHull2d currentHull = (ConvexHull2d)unprocessed.Dequeue();
                PlanePoint[] vertexes = currentHull.GetPoints().ToArray(); 
                for (int i = 0; i < vertexes.Length-1; i++)
                {
                    tempEdge.SetPoints(vertexes[i], vertexes[i+1]);
                    if (processedEdges.Contains(tempEdge))
                    {
                        continue;
                    }
                    processedEdges.Add((Edge2d) tempEdge.Clone());
                    PlanePoint orientPoint = vertexes[(i + 2) % vertexes.Length];
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;
                    for (int j = 0; j < points.Count; j++)
                    {
                        Hyperplane newHyperplane =
                            HyperplaneHelper.Create(new Point[]{vertexes[i], vertexes[i + 1], points[i]});
                        newHyperplane.SetOrientationNormals(orientPoint);
                        double newCos = currentHull.Hyperplane.Cos(newHyperplane);
                        if (Tools.GT(newCos, maxCos))
                        {
                            maxCos = newCos;
                            nextHyperplane = newHyperplane;
                        }
                        IList<PlanePoint> newPlanePoints = newHyperplane.GetPlanePoints(points);

                    }
                    


                }




            }

        }

        private ICell CreateSimplex(IList<PlanePoint> points)
        {
            throw new NotImplementedException();
        }

        private ConvexHull FindConvexHull(IList<Point> facePoints)
        {

            throw new NotImplementedException();
        }

        public ConvexHull2d FindConvexHull2D(IList<PlanePoint> points)
        {
            if (points.Count == 3)
            {
                return new ConvexHull2d(points);
            }
            ConvexHull2d convexHull = new ConvexHull2d();
            Point first = points.Min();
            Vector currentVector = new Vector(new double[] {0, -1});
            Point currentPoint = first;
            do
            {
                convexHull.AddPoint(currentPoint);
                double maxCos = double.MinValue;
                double maxLen = double.MinValue;
                Point next = currentPoint;
                Vector maxVector = currentVector;
                foreach (PlanePoint point in points)
                {
                    if (currentPoint == point) continue;
                    Vector newVector = Point.ToVector(currentPoint, point);
                    double newCos = currentVector * newVector;
                    newCos /= newVector.Length*currentVector.Length;
                    if (Tools.GT(newCos, maxCos))
                    {
                        maxCos = newCos;
                        next = point;
                        maxLen = Point.Length(currentPoint, next);
                        maxVector = newVector;
                    }
                    else if (Tools.EQ(maxCos, newCos))
                    {
                        double dist = Point.Length(currentPoint, point);
                        if (Tools.LT(maxLen, dist))
                        {
                            next = point;
                            maxVector = newVector;
                            maxLen = dist;
                        }
                    }
                }

                currentPoint = next;
                currentVector = maxVector;
            } while (first != currentPoint);

            return convexHull;
        }


        public ConvexHull2d FindConvexHull2Dv2(IList<Point> points)
        {
            ConvexHull2d convexHull = new ConvexHull2d();
            Point first = points.Min();
            Vector currentVector = new Vector(new double[] { 0, -1 });
            bool[] pointAvailability = new bool[points.Count];
            int current = points.IndexOf(first);
            do
            {
                convexHull.AddPoint(points[current]);
                double minAngle = double.MaxValue;
                double maxLen = double.MinValue;
                int next = current;
                Vector maxVector = currentVector;
                for (int i = 0; i < points.Count; i++)
                {
                    if (pointAvailability[i]||i==current) continue;
                    Vector newVector = Point.ToVector(points[current], points[i]);
                    double newAngle = Vector.Angle(currentVector, newVector);
                    if (Tools.LT(newAngle, minAngle))
                    {
                        minAngle = newAngle;
                        next = i;
                        maxLen = Point.Length(first, points[next]);
                        maxVector = newVector;
                    }
                    else if (Tools.EQ(minAngle, newAngle))
                    {
                        double length = Point.Length(points[current], points[i]);
                        if (Tools.LT(maxLen, length))
                        {
                            next = i;
                            maxVector = newVector;
                            maxLen = length;
                        }
                    }
                }

                current = next;
                currentVector = maxVector;
                pointAvailability[current] = true;
            } while (first != points[current]);

            return convexHull;
        }

        private IndexMap GetIndexMap(IList<Point> points)
        {
            return default;
        }

        protected Hyperplane FindFirstPlane(IList<PlanePoint> points)
        {
            PlaneFinder planeFinder = new PlaneFinder();

            return planeFinder.FindFirstPlane(points);
        }
    }
}