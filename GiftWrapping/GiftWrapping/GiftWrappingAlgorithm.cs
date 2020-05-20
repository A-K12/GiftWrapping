using System;
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

        private IList<PlanePoint> _points;

        private ICell _cell;
        public GiftWrappingAlgorithm(IList<Point> points, double tolerance)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("The number of _points must be more than three.");
            }
            _cell = new ConvexHull(points[0].Dim);
            this._points = points.Select(point => new PlanePoint(point)).ToArray();
        }

        public ConvexHull Create()
        {
            return FindConvexHull(_points);
        }

        protected ConvexHull FindConvexHull(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            ConvexHull convexHull = new ConvexHull(points[0].Dim);
            //Hyperplane hyperplane = FindFirstPlane(points);
            //IList<PlanePoint> planePoints = hyperplane.GetPlanePoints(points);
            //if (planePoints.Count == dim)
            //{
            //       return CreateSimplex(planePoints);
            //}
            //if (dim - 1 == 3)
            //{
            //    return FindConvexHull3D(points);
            //}
            //PlanePoint[] newPoints = planePoints.Select((point => hyperplane.ConvertPoint(point))).ToArray();
            //ICell currentFace = FindConvexHull(newPoints);
            //convexHull.InnerFaces.Add(currentFace);
            //Queue<ICell> unprocessedEdges = new Queue<ICell>();
            //unprocessedEdges.Enqueue(currentFace.);
            //while (unprocessedEdges.Count!=0)
            //{
            //    ICell edge = unprocessedEdges.Pop();
            //    Point[] edgePoints = new Point[edge.Hyperplane.Dimension+1];
            //    for (int i = 0; i < edgePoints.Length; i++)
            //    {
            //       // edgePoints[i] = edge.Hyperplane.Basis[i]; Вместо точек нужно брать вектора
            //    }
            //    double maxAngle = double.MinValue;
            //    Hyperplane maxHyperplane = currentFace.Hyperplane;
            //    for (int i = 0; i < points.Count; i++)
            //    {
            //        if(planePoints.Contains(points[i])) continue;
            //        edgePoints[^1] = points[i];
            //        IndexMap faceMap = GetIndexMap(edgePoints);//REMOVE
            //        Hyperplane newHyperplane = HyperplaneHelper.Create(edgePoints);
            //        double angle = currentFace.Hyperplane.Angle(newHyperplane);
            //        if (angle < maxAngle) continue; //if ==? 
            //        maxAngle = angle;
            //        maxHyperplane = newHyperplane;
            //    }

            //    if (convexHull.InnerFaces.Any((face => face.Hyperplane.Equals(maxHyperplane)))) continue;

            //    IList<Point> facePoints = maxHyperplane.GetPlanePoints(points);

            //    ConvexHull convexConvexHull = FindConvexHull(facePoints);
            //    convexHull.InnerFaces.Add(convexConvexHull);
            //    foreach (var face in convexConvexHull.InnerFaces)
            //    {
            //        unprocessedEdges.Push(face);
            //    }
            //}

            return convexHull;
        }

        public ConvexHull FindConvexHull3D(IList<PlanePoint> points)
        {
            ConvexHull convexHull = new ConvexHull(3);
            Queue<ICell> unprocessed = new Queue<ICell>();
            Dictionary<Edge2d, ConvexHull2d> processedEdges = new Dictionary<Edge2d, ConvexHull2d>();

            Hyperplane currentHyperplane = FindFirstPlane(points);
            IEnumerable<PlanePoint> originalPoints = points.Where(currentHyperplane.IsPointInPlane);
            IList<PlanePoint> planePoints2d = originalPoints.Select(currentHyperplane.ConvertPoint).ToList();
            ConvexHull2d currentHull = FindConvexHull2D(planePoints2d);
            currentHull.Hyperplane = currentHyperplane;
            convexHull.InnerFaces.Add(currentHull);


            unprocessed.Enqueue(currentHull);
            do
            {
                currentHull = (ConvexHull2d) unprocessed.Dequeue();
                originalPoints = points.Where(currentHull.Hyperplane.IsPointInPlane);
                foreach (Edge2d edge in Edge2d.GetEdges(currentHull))
                {
                    if (processedEdges.ContainsKey(edge))
                    {
                        if (processedEdges[edge] != default)
                        {
                            currentHull.AdjacentCells.Add(processedEdges[edge]);
                            processedEdges[edge].AdjacentCells.Add(currentHull);
                            processedEdges[edge] = default;
                        }
                        continue;
                    }
                 
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;
                    foreach (PlanePoint point in points)
                    {
                        if (originalPoints.Contains(point)) continue;
                        Point[] planePoints = new Point[] {edge.Points[0], edge.Points[1], point};
                        Hyperplane newHyperplane =
                            HyperplaneHelper.Create(planePoints);
                        newHyperplane.SetOrientationNormal(currentHull.GetPoints());
                        double newCos = currentHull.Hyperplane.Cos(newHyperplane);
                        if (Tools.GT(newCos, maxCos))
                        {
                            maxCos = newCos;
                            nextHyperplane = newHyperplane;
                        }
                    }
                    IList<PlanePoint> list = nextHyperplane.FindPoints(points);
                    ConvexHull2d newConvexHull = FindConvexHull2D(list);
                    newConvexHull.Hyperplane = nextHyperplane;
                    convexHull.InnerFaces.Add(newConvexHull);
                    newConvexHull.AdjacentCells.Add(currentHull);
                    currentHull.AdjacentCells.Add(newConvexHull);

                    foreach (Edge2d e in Edge2d.GetEdges(newConvexHull))
                    {
                        processedEdges[e] = newConvexHull;
                    }
                    processedEdges[(Edge2d)edge.Clone()] = default;
                    unprocessed.Enqueue(newConvexHull);
                }
            } while (unprocessed.Any());

            return convexHull;
        }

  
        private ICell CreateSimplex(IList<PlanePoint> points)
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
            PlanePoint first = points.Min();
            Vector currentVector = new Vector(new double[] {0, -1});
            PlanePoint currentPlanePoint = first;
            do
            {
                convexHull.AddPoint(currentPlanePoint.OriginalPoint);
                double maxCos = double.MinValue;
                double maxLen = double.MinValue;
                PlanePoint next = currentPlanePoint;
                Vector maxVector = currentVector;
                foreach (PlanePoint point in points)
                {
                    if (currentPlanePoint == point) continue;
                    Vector newVector = Point.ToVector(currentPlanePoint, point);
                    double newCos = currentVector * newVector;
                    newCos /= newVector.Length*currentVector.Length;
                    if (Tools.GT(newCos, maxCos))
                    {
                        maxCos = newCos;
                        next = point;
                        maxLen = Point.Length(currentPlanePoint, next);
                        maxVector = newVector;
                    }
                    else if (Tools.EQ(maxCos, newCos))
                    {
                        double dist = Point.Length(currentPlanePoint, point);
                        if (Tools.LT(maxLen, dist))
                        {
                            next = point;
                            maxVector = newVector;
                            maxLen = dist;
                        }
                    }
                }

                currentPlanePoint = next;
                currentVector = maxVector;
            } while (first != currentPlanePoint);

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

            //Todo ориентация первой нормали
        }
    }
}