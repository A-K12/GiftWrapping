using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class GiftWrappingAlgorithm_Test
    {
        private static IEnumerable GetNDimensionPoints()
        { 
            Point[] points = new Point[] {
                new Point(new double[]{0, 0, 0}),
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{4, 4, 0}),
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{4, 0, 4}),
                new Point(new double[]{0, 4, 4}),
                new Point(new double[]{4, 4, 4}),
                new Point(new double[]{1, 1, 1}),
                new Point(new double[]{2, 2, 2}),
            };

            Vector v1 = new Vector(new double[] { 0, 0, -1});
            Vector v2 = new Vector(new double[] { 0, -1, 0});
            Vector v3 = new Vector(new double[] { -1, 0, 0});
            Hyperplane[] expect = new Hyperplane[]
            {
                new Hyperplane(points[0], v1),
                new Hyperplane(points[0], v2),
                new Hyperplane(points[0], v3),
            };

            yield return new object[] { points, expect };
        }

        [Test, TestCaseSource(nameof(GetNDimensionPoints))]
        public void FindFirstPlane_Simplex_ReturnHyperplane(IList<Point> points, Hyperplane[] expected)
        {
            GiftWrappingAlgorithmTestClass giftWrapping = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);

            Hyperplane result = giftWrapping.FindFirstPlaneTest(points);

            expected.Should().Contain(result);
        }


        [Test]
        public void FindConvexHull2D_Points2d_ReturnConvexHull2D()
        {
            Point[] points = new Point[] {
                new Point(new double[]{4, 0}),
                new Point(new double[]{0, 4}),
                new Point(new double[]{4, 4}),
                new Point(new double[]{0, 0}),
                new Point(new double[]{0.5, 0.5}),
                new Point(new double[]{1, 1}),
            };
            List<Point> expectPoint = new List<Point>
            {
                new Point(new double[]{0, 0}),
                new Point(new double[]{4, 0}),
                new Point(new double[]{0, 4}),
                new Point(new double[]{4, 4}),
            };
            ConvexHull2d expect = expectPoint.ToConvexHull2d();

            GiftWrappingAlgorithmTestClass giftWrapping = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);
            
            ConvexHull2d actual = giftWrapping.FindConvexHull2D(points);

            actual.Should().Be(expect);
        }


        [Test, TestCaseSource("Get2dPoints")]
        public ConvexHull2d FindConvexHull2D_WhenCalls_ReturnsConvexHull(IList<Point> points)
        {
            GiftWrappingAlgorithmTestClass algorithm = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);

            return algorithm.FindConvexHull2D(points);
        }


        public static IEnumerable<TestCaseData> Get2dPoints()
        {
            List<Point> points = new List<Point> {
                new Point(new double[]{1, 5}),
                new Point(new double[]{1, 3}),
                new Point(new double[]{2, 1}),
                new Point(new double[]{4, 1.1}),
                new Point(new double[]{4, 0.5}),
                new Point(new double[]{5, 3}),
            };
            Point[] expectPoint = new Point[]{
                new Point(new double[]{1, 3}),
                new Point(new double[]{2, 1}),
                new Point(new double[]{4, 0.5}),
                new Point(new double[]{5, 3}),
                new Point(new double[]{1, 5}),
            };

            ConvexHull2d expect = expectPoint.ToConvexHull2d();

            yield return new TestCaseData(points).SetName("FindConvexHull2D_When2dPoint").Returns(expect);

            points = new List<Point> {
                new Point(new double[]{1, 1}),
                new Point(new double[]{1, 5}),
                new Point(new double[]{5, 1}),
                new Point(new double[]{7, 1}),
                new Point(new double[]{10, 1.1}),
                new Point(new double[]{10, 5}),
                new Point(new double[]{10, 8}),
                new Point(new double[]{10, 10}),

            };
            expectPoint = new Point[]{
                new Point(new double[]{1, 1}),
                new Point(new double[]{7, 1}),
                new Point(new double[]{10, 1.1}),
                new Point(new double[]{10, 10}),
                new Point(new double[]{1, 5}),
            };

            expect = expectPoint.ToConvexHull2d();

            yield return new TestCaseData(points).SetName("FindConvexHull2D_WhenMultiplePointsOnLine").Returns(expect);

        }
    }
}