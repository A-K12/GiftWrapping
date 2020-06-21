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
        private readonly IAlgorithm _algorithm2d,_algorithmSimplex; 

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
            _algorithmSimplex = new SimplexCreator();
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
                return _algorithmSimplex.FindConvexHull(points);
            }

            return FindConvexHullNd(points);
        }


        private IConvexHull FindConvexHullNd(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;

            List<IFace> faces = new List<IFace>();
            Dictionary<Hyperplane, IFace> processedPlanes = new Dictionary<Hyperplane, IFace>(new HyperplaneComparer());
          
            Hyperplane firstPlane = _planeFinder.FindFirstPlane(points);
            Face firstFace = new Face(firstPlane);

            processedPlanes.Add(firstPlane, firstFace);
            faces.Add(firstFace);
          
            for (int j = 0; j < faces.Count; j++)
            {
                IFace currentFace = faces[j];

                bool[] pointsMap = new bool[points.Count];
                List<PlanePoint> planePoints = new List<PlanePoint>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (!currentFace.Hyperplane.IsPointInPlane(points[i])) continue;
                    pointsMap[i] = true;
                    planePoints.Add(currentFace.Hyperplane.ConvertPoint(points[i]));
                }

                currentFace.ConvexHull = FindConvexHull(planePoints);
                
                if (planePoints.Count == points.Count)
                {
                    return currentFace.ConvexHull;
                }
                foreach (ICell edge in currentFace.ConvexHull.Cells)
                {
                    Hyperplane facePlane = currentFace.Hyperplane;
                    Vector innerVector = facePlane.ConvertVector(-edge.Hyperplane.Normal);
                    PlanePoint mainPoint = edge.Hyperplane.MainPoint.GetPoint(dim);
                    Vector[] edgeBasis = edge.Hyperplane.Basis.Select(facePlane.ConvertVector).ToArray();

                    double minCos = double.MaxValue;
                    Vector nextVector = default;
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (pointsMap[i]) continue;
                        Vector newVector = Point.ToVector(mainPoint, points[i]);
                        newVector = edgeBasis.GetOrthonormalVector(newVector);
                        if (Tools.EQ(newVector.Length)) continue;
                        double newCos = newVector.Cos(innerVector);
                        if (Tools.GT(newCos, minCos)) continue;
                        minCos = newCos;
                        nextVector = newVector;
                    }

                    Vector[] newFaceBasis = CreateFaceBasis(edgeBasis, nextVector);
                    Hyperplane nextHyperplane = Hyperplane.Create(mainPoint, newFaceBasis);
                    nextHyperplane.SetOrientationNormal(planePoints);

                    if (processedPlanes.TryGetValue(nextHyperplane, out IFace face))
                    {
                        face.AdjacentCells.Add(currentFace);
                        continue;
                    }
                    Face newFace = new Face(nextHyperplane);
                    newFace.AdjacentCells.Add(currentFace);

                    processedPlanes.Add(nextHyperplane, newFace);
                    faces.Add(newFace);
                }
            }

            ConvexHull convexHull = new ConvexHull(dim);
            faces.ForEach((face => convexHull.AddInnerCell(face)));

            return convexHull;
        }


        private Vector[] CreateFaceBasis(Vector[] edgeBasis, Vector faceVector)
        {
            Vector[] newFaceBasis = new Vector[edgeBasis.Length + 1];
            for (int i = 0; i < edgeBasis.Length; i++)
            {
                newFaceBasis[i] = edgeBasis[i];
            }
            newFaceBasis[^1] = faceVector;

            return newFaceBasis;
        }

    }
}