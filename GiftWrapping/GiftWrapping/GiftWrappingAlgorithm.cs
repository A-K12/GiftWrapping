using System;
using System.Collections;
using System.Collections.Generic;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class GiftWrappingAlgorithm
    {
        private double tolerance;
        public GiftWrappingAlgorithm(Point[] points, double tolerance)
        {
            if (points.Length < 3)
            {
                throw new ArgumentException("The number of points must be more than three.");
            }
            _convexFace = new ConvexFace(points[0].Dim);
            this.points = points;
        }

        private Point[] points;

        private IConvexFace _convexFace;

        public ConvexFace Create(List<Point> points)
        {
            FindFirstFace(points);
            return default;
        }

        protected void FindFirstFace(List<Point> points)
        {
            IndexMap mask = new IndexMap(points[0].Dim);
            FindFirstFace(points,mask);
        }
        protected IConvexFace FindFirstFace(IList<Point> points, IndexMap map)
        {
            if (map.Length == 2)
            {
                return FindConvexhull2D(points, map);
            }
            Hyperplane hyperplane = FindFirstPlane(points, map);
            IList<Point> planePoints = hyperplane.GetPlanePoints(points);
            IndexMap newMap = GetIndexMap(points);
            IConvexFace convexFace = FindFirstFace(planePoints, newMap);
            

        }

        private ConvexFace2D FindConvexhull2D(IList<Point> list, IndexMap map)
        {
            return new ConvexFace2D();
        }

        private IndexMap GetIndexMap(IList<Point> points)
        {
            return default;
        }

        protected Hyperplane FindFirstPlane(IList<Point> points, IndexMap mask)
        {
            int dim = mask.Length;                 
            PlaneFinder planeFinder = new PlaneFinder();

            return planeFinder.FindFirstPlane(points, mask);
        }
    }
}