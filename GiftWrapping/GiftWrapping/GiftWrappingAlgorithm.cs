using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private readonly IList<PlanePoint> _points;
        private readonly PlaneFinder _planeFinder;
        private readonly IAlgorithm _algorithm2d;
        public GiftWrappingAlgorithm(IList<PlanePoint> points)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("The number of _points must be more than three.");
            }
            _planeFinder = new PlaneFinder();
            _algorithm2d = new GiftWrapping2d();
            _points = points;
        }

        public IFace Create()
        {
            return FindConvexHull(_points);
        }

        private IFace FindConvexHull(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return _algorithm2d.FindConvexHull(points);
            }
            if (points.Count == dim + 1)
            {
                return CreateSimplex(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            Queue<(IFace, bool[])> unprocessedFaces = new Queue<(IFace, bool[])>();
            Dictionary<ICell, ICell> processedCells = new Dictionary<ICell, ICell>();
            Hyperplane currentHyperplane = _planeFinder.FindFirstPlane(points);
            currentHyperplane.OrthonormalBasis();
            List<PlanePoint> planePoints = new List<PlanePoint>();
            bool[] processedPoints = new bool[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                if (!currentHyperplane.IsPointInPlane(points[i])) continue;
                processedPoints[i] = true;
                planePoints.Add(currentHyperplane.ConvertPoint(points[i]));
            }
            Stopwatch sp = new Stopwatch();
            sp.Start();
            IFace currentHull = FindConvexHull(planePoints);
            sp.Stop();
            double first = sp.ElapsedMilliseconds;
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
                IEnumerable<PlanePoint> innerPoints = points.Where((_, i) => processedPoints[i]);
                //currentHull.InnerCells.Select((cell => cell.Hyperplane.MainPoint));
                foreach (ICell cell in currentHull.InnerCells)
                {
                    ICell adjacentCell = processedCells.GetValueOrDefault(cell);
                    if (ReferenceEquals(adjacentCell, null)) continue;
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;
                    foreach (Hyperplane hyperplane in GetHyperplanes(cell, currentHull, processedPoints, points))
                    {
                        hyperplane.SetOrientationNormal(innerPoints);
                        double newCos = currentHull.Hyperplane.Cos(hyperplane);
                        if (Tools.GT(newCos, maxCos))
                        {
                            maxCos = newCos;
                            nextHyperplane = hyperplane;
                        }
                    }
                    nextHyperplane.OrthonormalBasis();
                    planePoints.Clear();
                    bool[] newPointMap = new bool[points.Count];
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (!nextHyperplane.IsPointInPlane(points[j])) continue;
                        newPointMap[j] = true;
                        planePoints.Add(nextHyperplane.ConvertPoint(points[j]));
                    }
                    IFace newHull = FindConvexHull(planePoints);
                    newHull.Hyperplane = nextHyperplane;
                    convexHull.AddInnerCell(newHull);
                    foreach (ICell c in newHull.InnerCells)
                    {
                        ICell adj = processedCells.GetValueOrDefault(c);
                        if (!ReferenceEquals(null, adj))
                        {
                            UniteAdjacentCells(newHull, (IFace) adj);
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

        private IFace FindConvexHullv5(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return _algorithm2d.FindConvexHull(points); //n^2
            }
            if (points.Count == dim + 1)
            {
                return CreateSimplex(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            Queue<(ICell, bool[], IFace)> unprocessedFaces = new Queue<(ICell, bool[], IFace)>();
            Dictionary<ICell, ICell> processedCells = new Dictionary<ICell, ICell>();
            Hyperplane currentHyperplane = _planeFinder.FindFirstPlane(points); //O(dn)
            List<PlanePoint> planePoints = new List<PlanePoint>();
            bool[] processedPoints = new bool[points.Count];
            for (int i = 0; i < points.Count; i++) //n  O(dn)
            {
                if (!currentHyperplane.IsPointInPlane(points[i])) continue;
                processedPoints[i] = true;
                planePoints.Add(currentHyperplane.ConvertPoint(points[i]));// O(d)
            }
            Stopwatch sp = new Stopwatch();
            sp.Start();
            IFace currentHull = FindConvexHull(planePoints); //n^2
            sp.Stop();
            double first = sp.ElapsedMilliseconds;
            if (planePoints.Count == points.Count)
            {
                return currentHull;
            }
            currentHull.Hyperplane = currentHyperplane;
            convexHull.AddInnerCell(currentHull);
            foreach (ICell cell in currentHull.InnerCells)
            {
                processedCells.Add(cell, currentHull);
                unprocessedFaces.Enqueue((cell, processedPoints, currentHull));
            }
            while (unprocessedFaces.Any())
            {
                ICell cell;
                (cell, processedPoints, currentHull) = unprocessedFaces.Dequeue();
                IEnumerable<PlanePoint> innerPoints =
                    currentHull.InnerCells.Select((cell => cell.Hyperplane.MainPoint));
                ICell adjacentCell = processedCells.GetValueOrDefault(cell);
                if (ReferenceEquals(adjacentCell, null)) continue;
                double maxCos = double.MinValue;
                int len = cell.Hyperplane.Basis.Length;
                Vector[] basis = new Vector[len + 1];
                Array.Copy(cell.Hyperplane.Basis, 0, basis, 0, len);
                for (int i = 0; i < basis.Length - 1; i++)
                {
                    basis[i] = currentHull.Hyperplane.ConvertVector(basis[i]);
                }
                PlanePoint p = cell.Hyperplane.MainPoint.GetPoint(currentHull.Dimension + 1);
                int processedPoint = 0;
                Vector[] nextBasis = default;
                Vector nextNormal = default;
                for (int i = 0; i < points.Count; i++)
                {
                    if (processedPoints[i]) continue;
                    basis[^1] = Point.ToVector(p, points[i]);
                    Vector normal =  FindNormal(basis);
                    normal.SetOrientationNormal(p, innerPoints);
                    double newCos = currentHull.Hyperplane.Normal.Cos(normal);
                    if (Tools.LT(newCos, maxCos)) continue;
                    processedPoint = i;
                    maxCos = newCos;
                    nextNormal = normal;
                    nextBasis = (Vector[])basis.Clone();
                    
                }
                processedPoints[processedPoint] = true;
                Hyperplane nextHyperplane = new Hyperplane(p, nextNormal);
                nextHyperplane.Basis = nextBasis.GetOrthonormalBasis();
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
                    unprocessedFaces.Enqueue((c,newPointMap, newHull));
                }
            }
            return convexHull;
        }

        private IFace FindConvexHullv3(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return _algorithm2d.FindConvexHull(points);
            }
            if (points.Count == dim + 1)
            {
                return CreateSimplex(points);
            }
            Queue<(IFace, bool[])> unprocessedFaces = new Queue<(IFace, bool[])>();
            Dictionary<ICell, ICell> processedCells = new Dictionary<ICell, ICell>();
            Hyperplane currentHyperplane = _planeFinder.FindFirstPlane(points);
            List<PlanePoint> planePoints = new List<PlanePoint>();
            bool[] processedPoints = new bool[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                if (!currentHyperplane.IsPointInPlane(points[i])) continue;
                processedPoints[i] = true;
                planePoints.Add(currentHyperplane.ConvertPoint(points[i]));
            }
            Stopwatch sp = new Stopwatch();
            sp.Start();
            IFace currentHull = FindConvexHull(planePoints);
            sp.Stop();
            double first = sp.ElapsedMilliseconds;
            if (planePoints.Count == points.Count)
            {
                return currentHull;
            }
            ConvexHull convexHull = new ConvexHull(dim);
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
                IEnumerable<PlanePoint> innerPoints =
                currentHull.InnerCells.Select((cell => cell.Hyperplane.MainPoint));
                foreach (ICell cell in currentHull.InnerCells)
                {
                    ICell adjacentCell = processedCells.GetValueOrDefault(cell);
                    if (ReferenceEquals(adjacentCell, null)) continue;
                    double maxCos = double.MinValue;
                    Hyperplane nextHyperplane = currentHyperplane;

                    foreach (Hyperplane hyperplane in GetHyperplanes(cell, currentHull, processedPoints, points))
                    {
                        hyperplane.SetOrientationNormal(innerPoints);
                        double newCos = currentHull.Hyperplane.Cos(hyperplane);
                        if (Tools.GT(newCos, maxCos))
                        {
                            maxCos = newCos;
                            nextHyperplane = hyperplane;
                        }
                    }

                    planePoints.Clear();
                    bool[] newPointMap = new bool[points.Count];
                    // Stopwatch sp1 = new Stopwatch();
                    // sp1.Start();
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (!nextHyperplane.IsPointInPlane(points[j])) continue;
                        newPointMap[j] = true;
                        planePoints.Add(nextHyperplane.ConvertPoint(points[j]));
                    }

                    // sp1.Stop();
                    // long time = sp1.ElapsedMilliseconds;
                    // if (dim == 4)
                    // {
                    // }

                    IFace newHull = FindConvexHull(planePoints);
                    newHull.Hyperplane = nextHyperplane;
                    convexHull.AddInnerCell(newHull);
                    int processed = 0;
                    foreach (ICell c in newHull.InnerCells)
                    {
                        ICell adj = processedCells.GetValueOrDefault(c);
                        if (!ReferenceEquals(null, adj))
                        {
                            processed++;
                            UniteAdjacentCells(newHull, (IFace)adj);
                            processedCells[c] = default;
                            continue;
                        }
                        processedCells[c] = newHull;
                    }

                    if (newHull.InnerCells.Count != processed)
                    {
                        unprocessedFaces.Enqueue((newHull, newPointMap));
                    }
                  
                }
            }
            return convexHull;
        }

        private Vector FindNormal(Vector[] basis)
        {
            Matrix leftSide = basis.ToHorizontalMatrix();
            double[] rightSide = new double[basis.Length];

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
        }


        public IFace FindConvexHullV2(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return _algorithm2d.FindConvexHull(points);
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
                return _algorithm2d.FindConvexHull(points);
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



        private IndexMap GetIndexMap(IList<Point> points)
        {
            return default;
        }
    }
}