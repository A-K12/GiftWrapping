using System.Collections.Generic;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrappingTest.ConvexHull_test
{
    public class GiftWrappingAlgorithmTestClass:GiftWrappingAlgorithm
    {
        public GiftWrappingAlgorithmTestClass(IList<PlanePoint> points, double tolerance) : base(points)
        {
        }
        public void FindFirstFaceTest(IList<Point> points) => base.FindConvexHull(points.ToPlanePoint());
    }
}