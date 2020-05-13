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
        public GiftWrappingAlgorithm(IList<Point> points, double tolerance)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("The number of points must be more than three.");
            }
            _cell = new ConvexHull(points[0].Dim);
            this.points = points;
        }

        private IList<Point> points;

        private ICell _cell;

        public ConvexHull Create(List<Point> points)
        {
            FindFirstFace(points);
            return default;
        }

        protected void FindFirstFace(List<Point> points)
        {
            IndexMap mask = new IndexMap(points[0].Dim);
            FindFirstFace(points);
        }
        protected ICell FindFirstFace(IList<Point> points)
        {
            if (points[0].Dim == 2)
            {
                return FindConvexHull2D(points);
            }
            ConvexHull convexHull = new ConvexHull(points[0].Dim);
            Hyperplane hyperplane = FindFirstPlane(points);
            IList<Point> planePoints = hyperplane.GetPlanePoints(points);
            Point[] newPoints = hyperplane.RebuildPoints(planePoints);
            ICell currentFace = FindFirstFace(newPoints);
            convexHull.InnerFaces.Add(currentFace);
            Stack<ICell> unprocessedEdges = new Stack<ICell>();

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

        private ConvexHull FindConvexHull(IList<Point> facePoints)
        {

            throw new NotImplementedException();
        }

        public ConvexHull FindConvexHull2D(IList<Point> list)
        {
            ConvexHull convexHull = new ConvexHull(3);
            PlaneFinder pl = new PlaneFinder();

            Hyperplane hyperplane = pl.FindFirstPlane(list);
            hyperplane.ReorientNormal();
            IList<Point> planePoints = hyperplane.GetPlanePoints(list);
           // Edge2d firstEdge = new Edge2d(hyperplane);
            Point endPoint = planePoints.Min();
            //firstEdge.Points[0] = endPoint;
            //firstEdge.Points[1] = planePoints.Max(map);
         //   convexHull.InnerFaces.Add(firstEdge);
            while (true)
            {
                Edge2d currentEdge = (Edge2d)convexHull.InnerFaces.Last();
                Hyperplane maxHyperplane = default;
                double maxAngle = Double.MinValue;
                foreach (Point point in list)
                {
                    if (planePoints.Contains(point)) continue;
                    Point[] points = new Point[] { currentEdge.Points[1], point};
                    Hyperplane newHyperplane = HyperplaneHelper.Create(points);
                    //newHyperplane.ReorientNormal();
                    //double angle = currentEdge.Hyperplane.Angle(newHyperplane);
                    //if (angle < maxAngle) continue;
                    //maxAngle = angle;
                    //maxHyperplane = newHyperplane;
                }
                planePoints = maxHyperplane.GetPlanePoints(points);
                Point maxPoint = planePoints.Max();
                Point minPoint = planePoints.Min();
          //      Edge2d newEdge = new Edge2d(maxHyperplane);
                if(currentEdge.Points[1] == maxPoint)
                {
                    //newEdge.Points[0]= maxPoint;
                    //newEdge.Points[1] = minPoint;
                }
                else
                {
                    //newEdge.Points[0] = minPoint;
                    //newEdge.Points[1] = maxPoint;
                }
               // convexHull.InnerFaces.Add(newEdge);
               // if (newEdge.Points[1] == endPoint) break;
            }

            return convexHull;
        }

        private IndexMap GetIndexMap(IList<Point> points)
        {
            return default;
        }

        protected Hyperplane FindFirstPlane(IList<Point> points)
        {
            PlaneFinder planeFinder = new PlaneFinder();

            return planeFinder.FindFirstPlane(points);
        }
    }
}