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

namespace GiftWrappingTest.ConvexHull_test
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
            yield return new TestCaseData(points, expect).SetName("{m}_3dPoints");
        }

        [Test, TestCaseSource(nameof(GetNDimensionPoints)), Ignore("Not Working")]
        public void FindFirstPlane_ReturnHyperplane(IList<Point> points, Hyperplane[] expected)
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
        public ConvexHull2d FindConvexHull2D(IList<Point> points)
        {
            GiftWrappingAlgorithmTestClass algorithm = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);

            return algorithm.FindConvexHull2D(points);
        }


        public static IEnumerable<TestCaseData> Get2dPoints()
        {
            List<Point> points1 = new List<Point> {
                new Point(new double[]{1, 5}),
                new Point(new double[]{1, 3}),
                new Point(new double[]{2, 1}),
                new Point(new double[]{4, 1.1}),
                new Point(new double[]{4, 0.5}),
                new Point(new double[]{5, 3}),
            };
            Point[] expectPoint1 = new Point[]{
                new Point(new double[]{1, 3}),
                new Point(new double[]{2, 1}),
                new Point(new double[]{4, 0.5}),
                new Point(new double[]{5, 3}),
                new Point(new double[]{1, 5}),
            };

            ConvexHull2d expect1 = expectPoint1.ToConvexHull2d();

            yield return new TestCaseData(points1).SetName("{m}_When2dPoints").Returns(expect1);
            List <Point> points2 = new List<Point> {
                new Point(new double[]{1, 1}),
                new Point(new double[]{1, 5}),
                new Point(new double[]{5, 1}),
                new Point(new double[]{7, 1}),
                new Point(new double[]{10, 1.1}),
                new Point(new double[]{10, 5}),
                new Point(new double[]{10, 8}),
                new Point(new double[]{10, 10}),

            };
            Point[] expectPoint2 = new Point[]{
                new Point(new double[]{1, 1}),
                new Point(new double[]{7, 1}),
                new Point(new double[]{10, 1.1}),
                new Point(new double[]{10, 10}),
                new Point(new double[]{1, 5}),
            };

            ConvexHull2d expect2 = expectPoint2.ToConvexHull2d();

            yield return new TestCaseData(points2).SetName("{m}_WhenMultiplePointsOnLine").Returns(expect2);
           

            IEnumerable<Point> p1  = new Point[100];
            p1 = p1.Select(((_, i) => new Point(new double[] {1, i+1})));
            IEnumerable<Point> p2 = new Point[100];
            p2 = p2.Select(((_, i) => new Point(new double[] { i+1, 102 })));
            IEnumerable<Point> p3 = new Point[100];
            p3 = p3.Select(((_, i) => new Point(new double[] { 100, 101-i })));
            IEnumerable<Point> p4 = new Point[100];
            p4 = p4.Select(((_, i) => new Point(new double[] { 100-i, 0.5 })));
            List<Point> points3 = new List<Point>();
            points3.AddRange(p1);
            points3.AddRange(p2);
            points3.AddRange(p3);
            points3.AddRange(p4);
            Point[] expectPoint3 = new Point[]{
                new Point(new double[]{1, 0.5}),
                new Point(new double[]{100, 0.5}),
                new Point(new double[]{100, 102}),
                new Point(new double[]{1, 102}),
            };

            ConvexHull2d expect3 = expectPoint3.ToConvexHull2d();

            yield return new TestCaseData(points3).SetName("{m}_WhenMultiplePointsOnLine2").Returns(expect3);
            //SetName("FindConvexHull2D_WhenMultiplePointsOnLine")
        }


    }
}