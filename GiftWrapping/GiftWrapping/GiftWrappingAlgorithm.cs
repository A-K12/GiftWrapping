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
            bool[] processedPoints = new bool[points.Count];
            Queue<(IFace, bool[])> unprocessedFaces = new Queue<(IFace, bool[])>();
            Dictionary<Hyperplane, ICell> processedCells = new Dictionary<Hyperplane, ICell>();
            Hyperplane currentHyperplane = FindFirstPlane(points);
            IList<PlanePoint> planePoints2d = new List<PlanePoint>();
            for (int i = 0; i < points.Count; i++)
            {
                PlanePoint point = points[i];
                if (!currentHyperplane.IsPointInPlane(point)) continue;
                processedPoints[i] = true;
                planePoints2d.Add(currentHyperplane.ConvertPoint(point));
            }

            IFace currentHull = FindConvexHull(planePoints2d);
            IEnumerable<PlanePoint> hullPoints = currentHull.GetPoints().Select(point => point.GetPoint(dim));
           // processedPoints.ExceptWith(hullPoints);
            currentHull.Hyperplane = currentHyperplane;
            convexHull.AddInnerCell(currentHull);
            unprocessedFaces.Enqueue((currentHull, processedPoints));
            while (unprocessedFaces.Any())
            {
                (currentHull, processedPoints) = unprocessedFaces.Dequeue();
               // hullPoints = currentHull.GetPoints().Select(point => point.GetPoint(dim));
                foreach (ICell cell in currentHull.InnerCells)
                {
                  //  processedPoints.UnionWith(hullPoints);
                    if (processedCells.ContainsKey(cell.Hyperplane))
                    {
                        if (processedCells[cell.Hyperplane] != currentHull && processedCells[cell.Hyperplane] != default)
                        {
                            UniteAdjacentCells(currentHull, (IFace)processedCells[cell.Hyperplane]);
                            processedCells[cell.Hyperplane] = default;
                        }

                        if (processedCells[cell.Hyperplane] == default)
                        {
                            continue;
                        }
                    }
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;
                    foreach (Hyperplane hyperplane in GetHyperplanes(cell,currentHull, processedPoints, points))
                    {
                        hyperplane.SetOrientationNormal(currentHull.GetPoints());
                        double newCos = currentHull.Hyperplane.Cos(hyperplane);
                        if (Tools.LT(newCos, maxCos)) continue;
                        maxCos = newCos;
                        nextHyperplane = hyperplane;
                    }
                    planePoints2d.Clear();
                    bool[] newPointMap = new bool[points.Count];
                    for (int i = 0; i < points.Count; i++)
                    {
                        PlanePoint point = points[i];
                        if (!nextHyperplane.IsPointInPlane(point)) continue;
                        processedPoints[i] = true;
                        planePoints2d.Add(nextHyperplane.ConvertPoint(point));
                    }

                    ConvexHull2d newConvexHull = FindConvexHull2D(planePoints2d);
                    //IEnumerable<PlanePoint> newHullPoints = newConvexHull.GetPoints().Select(point => point.GetPoint(dim));
                    //processedPoints.ExceptWith(newHullPoints);
                    newConvexHull.Hyperplane = nextHyperplane;
                    convexHull.AddInnerCell(newConvexHull);
                    List<int> hash = new List<int>();
                    foreach (ICell c in newConvexHull.InnerCells)
                    {
                        if (!processedCells.ContainsKey(c.Hyperplane))
                        {
                            processedCells[c.Hyperplane] = newConvexHull;
                        }
                        hash.Add(c.Hyperplane.GetHashCode());
                    }
                    UniteAdjacentCells(currentHull, newConvexHull);
                    hash.Add(cell.GetHashCode());
                    if (processedCells.ContainsKey(cell.Hyperplane))
                    {
                        processedCells[cell.Hyperplane] = default;
                    }
                    processedCells[cell.Hyperplane] = default;
                    unprocessedFaces.Enqueue((newConvexHull, newPointMap));
                }
            }

            return convexHull;
        }

        private IEnumerable<Hyperplane> GetHyperplanes(ICell cell, ICell parent, bool[] processedPoints, IList<PlanePoint> points)
        {
            Hyperplane hyperplane = parent.Hyperplane;
            int len = cell.Hyperplane.Basis.Length;
            Vector[] basis = new Vector[len+1];
            Array.Copy(cell.Hyperplane.Basis,0,basis,0,len);
            for (int i = 0; i < basis.Length-1; i++)
            {
                basis[i] = hyperplane.ConvertVector(basis[i]);
            }
            PlanePoint p = cell.Hyperplane.MainPoint.GetPoint(parent.Dimension+1);
            for (int i = 0; i < points.Count; i++)
            {
                if (processedPoints[i]) continue;
                PlanePoint point = points[i];
                basis[^1] = Point.ToVector(p, point);
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
            List<PlanePoint> hullPoints = new List<PlanePoint>();
            PlanePoint first = points.Min();
            Vector currentVector = new Vector(new double[] {0, -1});
            PlanePoint currentPlanePoint = first;
            do
            {
                hullPoints.Add(currentPlanePoint);
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

            return new ConvexHull2d(hullPoints);
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