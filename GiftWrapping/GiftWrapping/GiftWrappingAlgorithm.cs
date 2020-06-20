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
            List<IFace> faces = new List<IFace>();
            Dictionary<Hyperplane, IFace> processedPlanes = new Dictionary<Hyperplane, IFace>(new HyperplaneComparer());
            Hyperplane firstPlane = _planeFinder.FindFirstPlane(points);
            ConvexHull firstFace = new ConvexHull(firstPlane);
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
                IFace newFace = FindConvexHull(planePoints);

                if (planePoints.Count == points.Count)
                {
                    return currentFace;
                }
                foreach (ICell cell in currentFace.InnerCells)
                {
                    double minCos = double.MaxValue;
                    Vector nextVector = default;
                    Hyperplane cellPlane = currentFace.Hyperplane;
                    Vector innerVector = cellPlane.ConvertVector(-cell.Hyperplane.Normal);
                    PlanePoint mainPoint = cellPlane.MainPoint.GetPoint(dim);
                    Vector[] edgeBasis = cell.Hyperplane.Basis.Select(cellPlane.ConvertVector).ToArray();

                    for (int i = 0; i < points.Count; i++)
                    {
                        if(processedPoints[i]) continue;
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
                    ConvexHull newConvexHull = new ConvexHull(nextHyperplane);
                    if (processedPlanes.TryGetValue(nextHyperplane,out IFace face))
                    {
                        face.AdjacentCells.Add(currentFace);
                        continue;
                    }
                    processedPlanes.Add(nextHyperplane, newConvexHull);
                    faces.Add(newConvexHull);
                    faces.Last().AdjacentCells.Add(currentFace);
                }
            }
            
            ConvexHull convexHull = new ConvexHull(dim);
            faces.ForEach((face => convexHull.AddInnerCell(face)));

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