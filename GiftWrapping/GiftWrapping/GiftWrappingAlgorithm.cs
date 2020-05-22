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

        public IFace Create()
        {
            return FindConvexHull(_points);
        }

        //protected ConvexHull FindConvexHull(IList<PlanePoint> points)
        //{
        //    int dim = points[0].Dim;
        //    ConvexHull convexHull = new ConvexHull(points[0].Dim);
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
            //convexHull.InnerCells.Add(currentFace);
            //Queue<ICell> unprocessedEdges = new Queue<ICell>();
            //unprocessedEdges.Enqueue(currentFace.);
            //while (unprocessedEdges.Count!=0)
            //{
            //    ICell cell = unprocessedEdges.Pop();
            //    Point[] edgePoints = new Point[cell.Hyperplane.Dimension+1];
            //    for (int i = 0; i < edgePoints.Length; i++)
            //    {
            //       // edgePoints[i] = cell.Hyperplane.Basis[i]; Вместо точек нужно брать вектора
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

            //    if (convexHull.InnerCells.Any((face => face.Hyperplane.Equals(maxHyperplane)))) continue;

            //    IList<Point> facePoints = maxHyperplane.GetPlanePoints(points);

            //    ConvexHull convexConvexHull = FindConvexHull(facePoints);
            //    convexHull.InnerCells.Add(convexConvexHull);
            //    foreach (var face in convexConvexHull.InnerCells)
            //    {
            //        unprocessedEdges.Push(face);
            //    }
            //}

        //    return convexHull;
        //}

        public IFace FindConvexHull(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return FindConvexHull2D(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            HashSet<PlanePoint> processedPoints = new HashSet<PlanePoint>();
            Queue<IFace> unprocessedFaces = new Queue<IFace>();
            Dictionary<ICell, ICell> processedCells = new Dictionary<ICell, ICell>();
            Hyperplane currentHyperplane = FindFirstPlane(points);
            IList<PlanePoint> planePoints2d = new List<PlanePoint>();
            foreach (PlanePoint point in points)
            {
                if (!currentHyperplane.IsPointInPlane(point)) continue;
                processedPoints.Add(point);
                planePoints2d.Add(currentHyperplane.ConvertPoint(point));
            }
            IFace currentHull = FindConvexHull(planePoints2d);

            processedPoints.ExceptWith(currentHull.GetPoints());
            currentHull.Hyperplane = currentHyperplane;
            convexHull.AddInnerCell(currentHull);
            unprocessedFaces.Enqueue(currentHull);
            while (unprocessedFaces.Any())
            {
                currentHull = unprocessedFaces.Dequeue();

                foreach (ICell cell in currentHull.InnerCells)
                {
                    processedPoints.UnionWith(currentHull.GetPoints());
                    if (processedCells.ContainsKey(cell))
                    {
                        if (processedCells[cell] != default)
                        {
                            UniteAdjacentCells(currentHull, (IFace)processedCells[cell]);
                            processedCells[cell] = default;
                        }
                        continue;
                    }
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;
                    foreach (Hyperplane hyperplane in GetHyperplanes(cell, processedPoints, points))
                    {
                        hyperplane.SetOrientationNormal(currentHull.GetPoints());
                        double newCos = currentHull.Hyperplane.Cos(hyperplane);
                        if (Tools.LT(newCos, maxCos)) continue;
                        maxCos = newCos;
                        nextHyperplane = hyperplane;
                    }
                    planePoints2d.Clear();
                    foreach (PlanePoint point in points)
                    {
                        if (!nextHyperplane.IsPointInPlane(point)) continue;
                        processedPoints.Add(point);
                        planePoints2d.Add(nextHyperplane.ConvertPoint(point));
                    }
                    ConvexHull2d newConvexHull = FindConvexHull2D(planePoints2d);
                    processedPoints.ExceptWith(newConvexHull.GetPoints());
                    newConvexHull.Hyperplane = nextHyperplane;
                    convexHull.AddInnerCell(newConvexHull);
                    foreach (ICell c in newConvexHull.InnerCells)
                    {
                        processedCells[c] = newConvexHull;
                    }
                    unprocessedFaces.Enqueue(newConvexHull);
                }
            }

            return convexHull;
        }

        private IEnumerable<Hyperplane> GetHyperplanes(ICell cell, HashSet<PlanePoint> processedPoints, IList<PlanePoint> points)
        {
            int dim = cell.Dimension;
            Vector[] basis = new Vector[dim+1];
            Array.Copy(cell.Hyperplane.Basis,0,basis,0,dim);
            PlanePoint p = cell.Hyperplane.MainPoint.OriginalPoint;
            foreach (PlanePoint point in points)
            {
                if (processedPoints.Contains(point)) continue;
                basis[^1] = Point.ToVector(point, p);
                Hyperplane newHyperplane =
                    HyperplaneHelper.Create(p, basis);

                yield return newHyperplane;
            }
        }

        private void UniteAdjacentCells(IFace c1, IFace c2)
        {
            c1.AddAdjacentCell(c2);
            c2.AddAdjacentCell(c1);
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
                convexHull.AddPoint(currentPlanePoint);
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