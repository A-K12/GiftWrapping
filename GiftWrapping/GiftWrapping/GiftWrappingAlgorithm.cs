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
        public GiftWrappingAlgorithm(IList<PlanePoint> points, double tolerance)
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

        public IFace FindConvexHull(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return FindConvexHull2D(points);
            }
            if (points.Count == dim + 1)
            {
                return CreateSimplex(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            Queue<(IFace, bool[])> unprocessedFaces = new Queue<(IFace, bool[])>();
            Dictionary<ICell, ICell> processedCells = new Dictionary<ICell, ICell>();
            Hyperplane currentHyperplane = FindFirstPlane(points);
            List<PlanePoint> planePoints = new List<PlanePoint>();
            bool[] processedPoints = new bool[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                if (!currentHyperplane.IsPointInPlane(points[i])) continue;
                processedPoints[i] = true;
                planePoints.Add(currentHyperplane.ConvertPoint(points[i]));
            }
            IFace currentHull = FindConvexHull(planePoints);
            if (planePoints.Count == points.Count)
            {
                return currentHull;
            }
            currentHull.Hyperplane = currentHyperplane;
            unprocessedFaces.Enqueue((currentHull, processedPoints));
            convexHull.AddInnerCell(currentHull);
            foreach (ICell cell in currentHull.InnerCells)
            {
                processedCells.Add(cell, currentHull);
            }
            while (unprocessedFaces.Any())
            {
                (currentHull, processedPoints) = unprocessedFaces.Dequeue();
                foreach (ICell cell in currentHull.InnerCells)
                {
                    ICell adjacentCell = processedCells.GetValueOrDefault(cell);
                    if (adjacentCell is null) continue;
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;
                    foreach (Hyperplane hyperplane in GetHyperplanes(cell,currentHull, processedPoints, points))
                    {
                        hyperplane.SetOrientationNormal(currentHull.GetPoints());
                        double newCos = currentHull.Hyperplane.Cos(hyperplane);
                        if (Tools.GT(newCos, maxCos))
                        {
                            maxCos = newCos;
                            nextHyperplane = hyperplane;
                        }
                    }
                    planePoints.Clear();
                    bool[] newPointMap = new bool[points.Count];
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (!nextHyperplane.IsPointInPlane(points[i])) continue;
                        newPointMap[i] = true;
                        planePoints.Add(nextHyperplane.ConvertPoint(points[i]));
                    }
                    IFace newHull = FindConvexHull(planePoints);
                    newHull.Hyperplane = nextHyperplane;
                    convexHull.AddInnerCell(newHull);
                    foreach (ICell c in newHull.InnerCells)
                    {
                        ICell adj = processedCells.GetValueOrDefault(c);
                        if (!ReferenceEquals(null, adj))
                        {
                            UniteAdjacentCells(newHull, (IFace)adj);
                            processedCells[c] = default;
                            continue;
                        }
                        processedCells[c] = newHull;
                    }
                    unprocessedFaces.Enqueue((newHull, newPointMap));
                }
              
            }

            return convexHull;
        }

        public IFace FindConvexHullV2(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return FindConvexHull2D(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            // Queue<(ICell, Hyperplane, bool[])> unprocessedFaces = new Queue<(ICell, Hyperplane, bool[])>();
            // Dictionary<ICell, ICell> processedCells = new Dictionary<ICell, ICell>();
            // Hyperplane currentHyperplane = FindFirstPlane(points);
            // List<PlanePoint> planePoints = new List<PlanePoint>();
            // bool[] processedPoints = new bool[points.Count];
            // for (int i = 0; i < points.Count; i++)
            // {
            //     if (!currentHyperplane.IsPointInPlane(points[i])) continue;
            //     processedPoints[i] = true;
            //     planePoints.Add(currentHyperplane.ConvertPoint(points[i]));
            // }
            // IFace currentHull = FindConvexHull(planePoints);
            // currentHull.Hyperplane = currentHyperplane;
            // foreach (ICell cell in currentHull.InnerCells)
            // {
            //     unprocessedFaces.Enqueue((cell,currentHyperplane, processedPoints));
            // }
            // convexHull.AddInnerCell(currentHull);
            // while (unprocessedFaces.Any())
            // {
            //     ICell edge;
            //     (edge, currentHyperplane, processedPoints) = unprocessedFaces.Dequeue();
            //     double minCos = double.MinValue;
            //     Hyperplane nextHyperplane = currentHyperplane;
            //     foreach (Hyperplane hyperplane in GetHyperplanes(edge, currentHyperplane, processedPoints, points))
            //     {
            //         hyperplane.SetOrientationNormal(currentHull.GetPoints());
            //         double newCos = currentHull.Hyperplane.Cos(hyperplane);
            //         if (Tools.LT(newCos, minCos)) continue;
            //         minCos = newCos;
            //         nextHyperplane = hyperplane;
            //     }
            //     planePoints.Clear();
            //     bool[] newPointMap = new bool[points.Count];
            //     for (int i = 0; i < points.Count; i++)
            //     {
            //         if (!nextHyperplane.IsPointInPlane(points[i])) continue;
            //         processedPoints[i] = true;
            //         planePoints.Add(nextHyperplane.ConvertPoint(points[i]));
            //     }
            //     ConvexHull2d newHull = FindConvexHull2D(planePoints);
            //     newHull.Hyperplane = nextHyperplane;
            //     convexHull.AddInnerCell(newHull);
            //     foreach (ICell c in newHull.InnerCells)
            //     {
            //         ICell adjCell = processedCells.GetValueOrDefault(c);
            //         if (!ReferenceEquals(null, adjCell))
            //         {
            //             UniteAdjacentCells(newHull, (IFace)adjCell);
            //             //processedCells[cell] = default;
            //             continue;
            //         }
            //         processedCells.Add(c, newHull);
            //         unprocessedFaces.Enqueue((c, nextHyperplane, newPointMap));
            //     }
            // }

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

        private IFace CreateSimplex(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return FindConvexHull2D(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            for (int i = 0; i < points.Count; i++)
            {
                List<PlanePoint> planePoints = points.Where((_, j) => j != i).ToList();
                Hyperplane hyperplane = HyperplaneHelper.Create(planePoints);
                List<PlanePoint> convertPoints = 
                    planePoints.Select((point => hyperplane.ConvertPoint(point))).ToList();
                IFace face = CreateSimplex(convertPoints);
                face.Hyperplane = hyperplane;
                foreach (ICell f in convexHull.InnerCells)
                {
                    UniteAdjacentCells(face, (IFace)f);
                }

                convexHull.AddInnerCell(face);
            }

            return convexHull;
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