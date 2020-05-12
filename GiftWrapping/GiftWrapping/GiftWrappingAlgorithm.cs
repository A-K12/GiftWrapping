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
            _cell = new ConvexHull(points[0].Dim);
            this.points = points;
        }

        private Point[] points;

        private ICell _cell;

        public ConvexHull Create(List<Point> points)
        {
            FindFirstFace(points);
            return default;
        }

        protected void FindFirstFace(List<Point> points)
        {
            IndexMap mask = new IndexMap(points[0].Dim);
            FindFirstFace(points,mask);
        }
        protected ICell FindFirstFace(IList<Point> points, IndexMap map)
        {
            if (map.Length == 2)
            {
                return FindConvexHull2D(points, map);
            }
            ConvexHull convexHull = new ConvexHull(points[0].Dim);
            Hyperplane hyperplane = FindFirstPlane(points, map);
            IList<Point> planePoints = hyperplane.GetPlanePoints(points);
            IndexMap newMap = GetIndexMap(points, map);
            ICell currentFace = FindFirstFace(planePoints, newMap);
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
                    IndexMap faceMap = GetIndexMap(edgePoints, map);
                    Hyperplane newHyperplane = HyperplaneHelper.Create(edgePoints, faceMap);
                    double angle = currentFace.Hyperplane.Angle(newHyperplane);
                    if (angle < maxAngle) continue; //if ==? 
                    maxAngle = angle;
                    maxHyperplane = newHyperplane;
                }

                if (convexHull.InnerFaces.Any((face => face.Hyperplane.Equals(maxHyperplane)))) continue;

                IList<Point> facePoints = maxHyperplane.GetPlanePoints(points);

                ConvexHull convexConvexHull = FindConvexHull(facePoints, maxHyperplane.Mask);
                convexHull.InnerFaces.Add(convexConvexHull);
                foreach (var face in convexConvexHull.InnerFaces)
                {
                    unprocessedEdges.Push(face);
                }
            }

            return convexHull;
        }

        private ConvexHull FindConvexHull(IList<Point> facePoints, IndexMap maxHyperplaneMask)
        {

            throw new NotImplementedException();
        }

        public ConvexHull FindConvexHull2D(IList<Point> list, IndexMap map)
        {
            ConvexHull convexHull = new ConvexHull(3);
            PlaneFinder pl = new PlaneFinder();

            Hyperplane hyperplane = pl.FindFirstPlane(list, map);
            hyperplane.ReorientNormal();
            IList<Point> planePoints = hyperplane.GetPlanePoints(list);
           // Edge2d firstEdge = new Edge2d(hyperplane);
            Point endPoint = planePoints.Min(map);
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
                    Hyperplane newHyperplane = HyperplaneHelper.Create(points, map);
                    //newHyperplane.ReorientNormal();
                    //double angle = currentEdge.Hyperplane.Angle(newHyperplane);
                    //if (angle < maxAngle) continue;
                    //maxAngle = angle;
                    //maxHyperplane = newHyperplane;
                }
                planePoints = maxHyperplane.GetPlanePoints(points);
                Point maxPoint = planePoints.Max(map);
                Point minPoint = planePoints.Min(map);
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