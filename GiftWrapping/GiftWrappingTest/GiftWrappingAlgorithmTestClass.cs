using System.Collections.Generic;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrappingTest
{
    public class GiftWrappingAlgorithmTestClass:GiftWrappingAlgorithm
    {
        public GiftWrappingAlgorithmTestClass(Point[] points, double tolerance) : base(points, tolerance)
        {
        }
        public void FindFirstFaceTest(List<Point> points) => base.FindFirstFace(points);

        public Hyperplane FindFirstPlaneTest(List<Point> points, IndexMap mask) => base.FindFirstPlane(points, mask);
    }
}