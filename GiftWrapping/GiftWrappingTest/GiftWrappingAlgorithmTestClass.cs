using System.Collections.Generic;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrappingTest
{
    public class GiftWrappingAlgorithmTestClass:GiftWrappingAlgorithm
    {
        public GiftWrappingAlgorithmTestClass(IList<Point> points, double tolerance) : base(points, tolerance)
        {
        }
        public void FindFirstFaceTest(IList<Point> points) => base.FindFirstFace(points);

        public Hyperplane FindFirstPlaneTest(IList<Point> points) => base.FindFirstPlane(points);
    }
}