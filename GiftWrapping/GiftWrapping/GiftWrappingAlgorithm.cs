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

        public IConvexHull Create()
        {
            return FindConvexHull(_points);
        }

        private IConvexHull FindConvexHull(IList<PlanePoint> points)
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
            List<IFace> faces = new List<IFace>();
            Dictionary<Hyperplane, IFace> processedPlanes = new Dictionary<Hyperplane, IFace>(new HyperplaneComparer());
            Hyperplane firstPlane = _planeFinder.FindFirstPlane(points);
            Face firstFace = new Face(firstPlane);
            processedPlanes.Add(firstPlane, firstFace);
            faces.Add(firstFace);
            for (int j = 0; j < faces.Count; j++)
            {
                IFace currentFace = faces[j];
                bool[] processedPoints = new bool[points.Count];
                List<PlanePoint> planePoints = new List<PlanePoint>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (!currentFace.Hyperplane.IsPointInPlane(points[i])) continue;
                    processedPoints[i] = true;
                    planePoints.Add(currentFace.Hyperplane.ConvertPoint(points[i]));
                }
                IConvexHull newConvexHull = FindConvexHull(planePoints);
                if (planePoints.Count == points.Count)
                {
                    return newConvexHull;
                }
                currentFace.ConvexHull = newConvexHull;
                foreach (ICell cell in currentFace.ConvexHull.Faces)
                {
                    double minCos = double.MaxValue;
                    Vector nextVector = default;
                    Hyperplane cellPlane = currentFace.Hyperplane;
                    Vector innerVector = cellPlane.ConvertVector(-cell.Hyperplane.Normal);
                    PlanePoint mainPoint = cell.Hyperplane.MainPoint.GetPoint(dim);
                    Vector[] edgeBasis = cell.Hyperplane.Basis.Select(cellPlane.ConvertVector).ToArray();

                    for (int i = 0; i < points.Count; i++)
                    {
                        if (processedPoints[i]) continue;
                        Vector newVector = Point.ToVector(mainPoint, points[i]);
                        newVector = edgeBasis.GetOrthonormalVector(newVector);
                        if (Tools.EQ(newVector.Length)) continue;
                        double newCos = newVector.Cos(innerVector);
                        if (Tools.GT(newCos, minCos)) continue;
                        minCos = newCos;
                        nextVector = newVector;
                    }
                    Vector[] newFaceBasis = new Vector[edgeBasis.Length+1];
                    for (int i = 0; i < edgeBasis.Length; i++)
                    {
                        newFaceBasis[i] = edgeBasis[i];
                    }
                    newFaceBasis[^1] = nextVector;
                    Hyperplane nextHyperplane = HyperplaneBuilder.Create(mainPoint, newFaceBasis);
                    nextHyperplane.SetOrientationNormal(planePoints);
                    if (processedPlanes.TryGetValue(nextHyperplane,out IFace face))
                    {
                        face.AdjacentCells.Add(currentFace);
                        continue;
                    }

                    Face newFace = new Face(nextHyperplane);
                    processedPlanes.Add(nextHyperplane, newFace);
                    faces.Add(newFace);
                    faces.Last().AdjacentCells.Add(currentFace);
                }
            }
            
            ConvexHull convexHull = new ConvexHull(dim);
            faces.ForEach((face => convexHull.AddInnerCell(face)));

            return convexHull;
        }

        private IConvexHull CreateSimplex(IList<PlanePoint> points)
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
                Face newFace = new Face(hyperplane);
                newFace.ConvexHull = CreateSimplex(convertPoints);
                foreach (ICell f in newFace.ConvexHull.Faces)
                {
                    newFace.AdjacentCells.Add((IFace)f);
                    ((IFace)f).AdjacentCells.Add(newFace);
                }

                convexHull.AddInnerCell(newFace);
            }

            return convexHull;
        }


    }
}