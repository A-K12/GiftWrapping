using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class GiftWrappingAlgorithm
    {
        private double tolerance;
        public GiftWrappingAlgorithm(Point[] points, double tolerance)
        {
            if (points.Length < 3)
            {
                throw new ArgumentException("The number of points must be more than three.");
            }
            _convexFace = new ConvexFace(points[0].Dim);
            this.points = points;
        }

        private Point[] points;

        private IConvexFace _convexFace;

        public ConvexFace Create(List<Point> points)
        {
            FindFirstFace(points);
            return default;
        }

        protected void FindFirstFace(List<Point> points)
        {
            IndexMap mask = new IndexMap(points[0].Dim);
            FindFirstFace(points,mask);
        }
        protected IConvexFace FindFirstFace(IList<Point> points, IndexMap map)
        {
            if (map.Length == 2)
            {
                return FindConvexHull2D(points, map);
            }
            ConvexFace convexHull = new ConvexFace(points[0].Dim);
            Hyperplane hyperplane = FindFirstPlane(points, map);
            IList<Point> planePoints = hyperplane.GetPlanePoints(points);
            IndexMap newMap = GetIndexMap(points, map);
            IConvexFace currentFace = FindFirstFace(planePoints, newMap);
            convexHull.InnerFaces.Add(currentFace);
            Stack<IConvexFace> unprocessedEdges = new Stack<IConvexFace>();

            while (unprocessedEdges.Count!=0)
            {
                IConvexFace edge = unprocessedEdges.Pop();
                Point[] edgePoints = new Point[edge.Hyperplane.Dim+1];
                for (int i = 0; i < edgePoints.Length; i++)
                {
                    edgePoints[i] = edge.Hyperplane.Points[i];
                }
                double maxAngle = double.MinValue;
                Hyperplane maxHyperplane = currentFace.Hyperplane;
                for (int i = 0; i < points.Count; i++)
                {
                    if(planePoints.Contains(points[i])) continue;
                    edgePoints[^1] = points[i];
                    IndexMap faceMap = GetIndexMap(edgePoints, map);
                    Hyperplane newHyperplane = HyperplaneHelper.Create(edgePoints, faceMap);
                    double angle = currentFace.Hyperplane.Angle(newHyperplane);
                    if (angle < maxAngle) continue; //if ==? 
                    maxAngle = angle;
                    maxHyperplane = newHyperplane;
                }

                if (convexHull.InnerFaces.Any((face => face.Hyperplane.Equals(maxHyperplane)))) continue;

                IList<Point> facePoints = maxHyperplane.GetPlanePoints(points);

                ConvexFace convexFace = FindConvexHull(facePoints, maxHyperplane.Mask);
                convexHull.InnerFaces.Add(convexFace);
                foreach (var face in convexFace.InnerFaces)
                {
                    unprocessedEdges.Push(face);
                }
            }

            return convexHull;
        }

        private ConvexFace FindConvexHull(IList<Point> facePoints, IndexMap maxHyperplaneMask)
        {

            throw new NotImplementedException();
        }

        public ConvexFace2D FindConvexHull2D(IList<Point> list, IndexMap map)
        {
            ConvexFace2D conveHull = new ConvexFace2D();
            PlaneFinder pl = new PlaneFinder();
            Hyperplane hyperplane = pl.FindFirstPlane(list, map);
            IList<Point> planePoints = hyperplane.GetPlanePoints(list);
            conveHull.Points.Add(planePoints.Min());
            Point point = planePoints.Max();
            conveHull.Points.Add(point);
            
            Hyperplane maxHyperplane = default;
            double maxAngle = Double.MinValue;
            while (true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (planePoints.Contains(list[i])) continue;
                    Point[] points = new Point[] { point, list[i] };
                    Hyperplane newHyperplane = HyperplaneHelper.Create(points, map);
                    newHyperplane.ReorientNormal();
                    double angle = hyperplane.Angle(newHyperplane);
                    if (angle < maxAngle) continue;
                    maxAngle = angle;
                    maxHyperplane = newHyperplane;
                }
                planePoints = maxHyperplane.GetPlanePoints(points);
                point = planePoints.Max();
                Point minPoint = planePoints.Min();
                if(conveHull.Points.Contains(point))
                {
                    if (conveHull.Points.Contains(minPoint))
                    {
                        break;
                    }
                    conveHull.Points.Add(minPoint);
                }
                else
                {
                    conveHull.Points.Add(point);
                }
                hyperplane = maxHyperplane;
            }

            return conveHull;
        }

        private IndexMap GetIndexMap(IList<Point> points, IndexMap indexMap)
        {
            return default;
        }

        protected Hyperplane FindFirstPlane(IList<Point> points, IndexMap mask)
        {
            int dim = mask.Length;                 
            PlaneFinder planeFinder = new PlaneFinder();

            return planeFinder.FindFirstPlane(points, mask);
        }
    }
}