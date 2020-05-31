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
    [TestFixture]
    public class GiftWrappingAlgorithm_Test
    {
     
        private static IEnumerable GetNDimensionPoints()
        {
            PlanePoint[] points = new PlanePoint[] {
                new PlanePoint(new double[]{1, 1, 0}),
                new PlanePoint(new double[]{5, 1, 0}),
                new PlanePoint(new double[]{1, 5, 0}),
                new PlanePoint(new double[]{5, 5, 0}),
                new PlanePoint(new double[]{1, 1, 5}),
                new PlanePoint(new double[]{5, 1, 5}),
                new PlanePoint(new double[]{1, 5, 5}),
                new PlanePoint(new double[]{5, 5, 5}),
                new PlanePoint(new double[]{3, 3, 3}),
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
            yield return new TestCaseData(points).SetName("{m}_3dPoints");
        }

        [Test]
        public void FindFirstPlane_ReturnHyperplane()
        {
            Point[] points = new Point[] {
                new Point(new double[]{1, 1, 0}),
                new Point(new double[]{5, 1, 0}),
                new Point(new double[]{1, 5, 0}),
                new Point(new double[]{5, 5, 0}),
                new Point(new double[]{1, 1, 5}),
                new Point(new double[]{5, 1, 5}),
                new Point(new double[]{1, 5, 5}),
                new Point(new double[]{5, 5, 5}),
                new Point(new double[]{3, 3, 3}),
            };

            GiftWrappingAlgorithmTestClass giftWrapping = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);

            IFace result = giftWrapping.FindConvexHull(points.ToPlanePoint());

            Assert.IsTrue(false);
        }


        [Test]
        public void FindConvexHull2D_Points2d_ReturnConvexHull2D()
        {
            PlanePoint[] points = new PlanePoint[] {
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{0.5, 0.5}),
                new PlanePoint(new double[]{1, 1}),
            };
            List<PlanePoint> expectPoint = new List<PlanePoint>
            {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
            };
            ConvexHull2d expect = expectPoint.ToConvexHull2d();

            GiftWrappingAlgorithmTestClass giftWrapping = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);
            
            ConvexHull2d actual = giftWrapping.FindConvexHull2D(points.ToPlanePoint());

            actual.Should().Be(expect);
        }


        [Test, TestCaseSource("Get2dPoints")]
        public ConvexHull2d FindConvexHull2D(IList<Point> points)
        {
            GiftWrappingAlgorithmTestClass algorithm = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);

            return algorithm.FindConvexHull2D(points.ToPlanePoint());
        }


        public static IEnumerable<TestCaseData> Get2dPoints()
        {
            List<PlanePoint> points1 = new List<PlanePoint> {
                new PlanePoint(new double[]{1, 5}),
                new PlanePoint(new double[]{1, 3}),
                new PlanePoint(new double[]{2, 1}),
                new PlanePoint(new double[]{4, 1.1}),
                new PlanePoint(new double[]{4, 0.5}),
                new PlanePoint(new double[]{5, 3}),
            };
            PlanePoint[] expectPoint1 = new PlanePoint[]{
                new PlanePoint(new double[]{1, 3}),
                new PlanePoint(new double[]{2, 1}),
                new PlanePoint(new double[]{4, 0.5}),
                new PlanePoint(new double[]{5, 3}),
                new PlanePoint(new double[]{1, 5}),
            };

            ConvexHull2d expect1 = expectPoint1.ToConvexHull2d();

            yield return new TestCaseData(points1).SetName("{m}_When2dPoints").Returns(expect1);
            List <PlanePoint> points2 = new List<PlanePoint> {
                new PlanePoint(new double[]{1, 1}),
                new PlanePoint(new double[]{1, 5}),
                new PlanePoint(new double[]{5, 1}),
                new PlanePoint(new double[]{7, 1}),
                new PlanePoint(new double[]{10, 1.1}),
                new PlanePoint(new double[]{10, 5}),
                new PlanePoint(new double[]{10, 8}),
                new PlanePoint(new double[]{10, 10}),

            };
            PlanePoint[] expectPoint2 = new PlanePoint[]{
                new PlanePoint(new double[]{1, 1}),
                new PlanePoint(new double[]{7, 1}),
                new PlanePoint(new double[]{10, 1.1}),
                new PlanePoint(new double[]{10, 10}),
                new PlanePoint(new double[]{1, 5}),
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
            PlanePoint[] expectPoint3 = new PlanePoint[]{
                new PlanePoint(new double[]{1, 0.5}),
                new PlanePoint(new double[]{100, 0.5}),
                new PlanePoint(new double[]{100, 102}),
                new PlanePoint(new double[]{1, 102}),
            };

            ConvexHull2d expect3 = expectPoint3.ToConvexHull2d();

            yield return new TestCaseData(points3).SetName("{m}_WhenMultiplePointsOnLine2").Returns(expect3);
            //SetName("FindConvexHull2D_WhenMultiplePointsOnLine")
        }


    }
}