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

            if (!points.HaveSameDimension())
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
            Queue<Hyperplane> unprocessedFaces = new Queue<Hyperplane>();
            Dictionary<Hyperplane, int> processedCells = new Dictionary<Hyperplane, int>(new HyperplaneComparer());
            List<List<IFace>> adjacentFaces = new List<List<IFace>>();
            Hyperplane currentHyperplane = _planeFinder.FindFirstPlane(points);
            List<PlanePoint> planePoints = new List<PlanePoint>();
            ConvexHull convexHull = new ConvexHull(dim);
            unprocessedFaces.Enqueue(currentHyperplane);
            int index = 0;
            processedCells.Add(currentHyperplane, index++);
            adjacentFaces.Add(new List<IFace>());
            while (unprocessedFaces.Any())
            {
                currentHyperplane = unprocessedFaces.Dequeue();
                bool[] processedPoints = new bool[points.Count];
                planePoints.Clear();
                for (int i = 0; i < points.Count; i++)
                {
                    if (!currentHyperplane.IsPointInPlane(points[i])) continue;
                    processedPoints[i] = true;
                    planePoints.Add(currentHyperplane.ConvertPoint(points[i]));
                }
                IFace currentHull = FindConvexHull(planePoints);
                currentHull.Hyperplane = currentHyperplane;
                convexHull.AddInnerCell(currentHull);
                if (planePoints.Count == points.Count)
                {
                    return convexHull;
                }
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
                    nextHyperplane.SetOrientationNormal(planePoints);
                    
                    if (processedCells.TryGetValue(nextHyperplane,out int adjacentCell))
                    {
                        adjacentFaces[adjacentCell].Add(currentHull);
                        continue;
                    }
                    processedCells.Add(nextHyperplane, index++);
                    adjacentFaces.Add(new List<IFace>());
                    adjacentFaces.Last().Add(currentHull);
                    unprocessedFaces.Enqueue(nextHyperplane);
                 
                }

            }

            for (int i = 0; i < adjacentFaces.Count; i++)
            {
                IFace face = (IFace)convexHull.InnerCells[i];
                face.AdjacentCells.AddRange(adjacentFaces[i]);
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