using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

            if (points.HaveSameDimension())
            {
                throw new ArgumentException("Points have different dimensions.");
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
          
            Queue<(IFace, bool[])> unprocessedFaces = new Queue<(IFace, bool[])>();
            Dictionary<Hyperplane, ICell> processedCells = new Dictionary<Hyperplane, ICell>(new VectorComparer());
            Hyperplane currentHyperplane = _planeFinder.FindFirstPlane(points);
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
            ConvexHull convexHull = new ConvexHull(dim);
            currentHull.Hyperplane = currentHyperplane;
            unprocessedFaces.Enqueue((currentHull, processedPoints));
            convexHull.AddInnerCell(currentHull);
            processedCells.Add(currentHull.Hyperplane, currentHull);

            while (unprocessedFaces.Any())
            {
                (currentHull, processedPoints) = unprocessedFaces.Dequeue();
                IEnumerable<PlanePoint> innerPoints = currentHull.GetPoints();
                foreach (ICell cell in currentHull.InnerCells)
                {
                    double minCos = double.MaxValue;
                    Vector nextVector = default;
                    Hyperplane cellPlane = cell.Hyperplane;
                    Vector innerVector = currentHull.Hyperplane.ConvertVector(-cell.Hyperplane.Normal);
                    PlanePoint mainPoint = cellPlane.MainPoint.GetPoint(dim);
                    Vector[] basis = cellPlane.Basis.Select(currentHull.Hyperplane.ConvertVector).ToArray();

                    for (int i = 0; i < points.Count; i++)
                    {
                        if(processedPoints[i]) continue;
                        Vector newVector = Point.ToVector(mainPoint, points[i]);
                        newVector = basis.GetOrthonormalVector(newVector);
                        if (Tools.EQ(newVector.Length)) continue;
                        double newCos = newVector.Cos(innerVector);
                        if (Tools.GT(newCos, minCos)) continue;
                        minCos = newCos;
                        nextVector = newVector;
                    }
                    Vector[] newBasis = new Vector[basis.Length+1];
                    for (int i = 0; i < cellPlane.Basis.Length; i++)
                    {
                        newBasis[i] = basis[i];
                    }
                    newBasis[^1] = nextVector;

                    Hyperplane nextHyperplane = HyperplaneBuilder.Create(mainPoint, newBasis);
                    nextHyperplane.SetOrientationNormal(innerPoints);
                    ICell adjacentCell = processedCells.GetValueOrDefault(nextHyperplane);
                    if (adjacentCell is { })
                    {
                        currentHull.AddAdjacentCell(adjacentCell);
                        continue;
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
                    currentHull.AddAdjacentCell(newHull);

                    processedCells.Add(nextHyperplane, newHull);

                    unprocessedFaces.Enqueue((newHull, newPointMap));
                }
            }
            return convexHull;
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
                Hyperplane hyperplane = HyperplaneBuilder.Create(planePoints);
                List<PlanePoint> convertPoints = 
                    planePoints.Select((point => hyperplane.ConvertPoint(point))).ToList();
                IFace face = CreateSimplex(convertPoints);
                face.Hyperplane = hyperplane;
                foreach (ICell f in convexHull.InnerCells)
                {
                    face.AddAdjacentCell((IFace)f);
                    ((IFace)f).AddAdjacentCell(face);
                }

                convexHull.AddInnerCell(face);
            }

            return convexHull;
        }


    }
}