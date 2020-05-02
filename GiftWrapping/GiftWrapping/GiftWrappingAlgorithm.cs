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
            this.points = points;
            faces = new List<IConvexFace>();
            unprocessedFaces = new Queue<IConvexFace>();
        }

        private Point[] points;

        private IList<IConvexFace> faces;

        private Queue<IConvexFace> unprocessedFaces;

        public ConvexFace Create(List<Point> points)
        {
            FindFirstFace(points);

            return new ConvexFace();
        }

        protected void FindFirstFace(List<Point> points)
        {
            IndexMap mask = new IndexMap(points[0].Dim);
            FindFirstFace(points,mask);


        }
        protected void FindFirstFace(List<Point> points, IndexMap map)
        {
            Hyperplane hyperplane = FindFirstPlane(points, map);
            IList<Point> planePoints = hyperplane.GetPlanePoints(points);
            if (planePoints[0].Dim == 2)
            {
                //Find ConvexHull
            }//planePoint!=0
            IndexMap newMap = GetIndexMap(points);


        }

        private IndexMap GetIndexMap(IList<Point> points)
        {
            return default;
        }

        protected Hyperplane FindFirstPlane(List<Point> points, IndexMap mask)
        {
            int dim = mask.Length;                 
            PlaneFinder planeFinder = new PlaneFinder();

            return planeFinder.FindFirstPlane(points, mask);
        }
    }
}