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
        private static IEnumerable SetPoints()
        { 
            Point[] points = new Point[] {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{4, 4, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{4, 0, 4, 0}),
                new Point(new double[]{0, 4, 4, 0}),
                new Point(new double[]{4, 4, 4, 0}),
                new Point(new double[]{1, 1, 1, 0}),
                new Point(new double[]{2, 2, 2, 0}),
            };

            Vector v1 = new Vector(new double[] { 0, 0, -1});
            Vector v2 = new Vector(new double[] { 0, -1, 0});
            Vector v3 = new Vector(new double[] { -1, 0, 0});
            IndexMap map = new IndexMap(points[0].Dim- 1);
            Hyperplane[] expect = new Hyperplane[]
            {
                new Hyperplane(points[0], v1, map),
                new Hyperplane(points[0], v2, map),
                new Hyperplane(points[0], v3, map),
            };

            yield return new object[] { points, expect, map };

            points = new Point[] {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{4, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 4}),
                new Point(new double[]{4, 0, 0, 4}),
                new Point(new double[]{0, 0, 4, 4}),
                new Point(new double[]{4, 0, 4, 4}),
                new Point(new double[]{1, 0, 1, 1}),
                new Point(new double[]{2, 0, 2, 2}),
            };

            v1 = new Vector(new double[] { 0, 0, -1 });
            v2 = new Vector(new double[] { 0, -1, 0 });
            v3 = new Vector(new double[] { -1, 0, 0 });
            map = new IndexMap(new int[]{0,2,3});
            expect = new Hyperplane[]
            {
                new Hyperplane(points[0], v1, map),
                new Hyperplane(points[0], v2, map),
                new Hyperplane(points[0], v3, map),
            };

            yield return new object[] { points, expect, map };

        }

        [Test, TestCaseSource(nameof(SetPoints))]
        public void FindFirstPlane_Simplex_ReturnHyperplane(Point[] points, Hyperplane[] expected, IndexMap map)
        {
            GiftWrappingAlgorithmTestClass giftWrapping = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);

            Hyperplane result = giftWrapping.FindFirstPlaneTest(points.ToList(), map);

            expected.Should().Contain(result);
        }


        [Test, Ignore("Not working")]
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
            List<Point> exceptPoint = new List<Point> {
                new Point(new double[]{0, 0}),
                new Point(new double[]{4, 0}),
                new Point(new double[]{0, 4}),
                new Point(new double[]{4, 4}),
            };
            ConvexHull except = exceptPoint.ToConvexHull2d();

            GiftWrappingAlgorithmTestClass giftWrapping = new GiftWrappingAlgorithmTestClass(points, Tools.Eps);
            
            ConvexHull actual = giftWrapping.FindConvexHull2D(points, new IndexMap(2));

            actual.Should().Be(except);
        }
    }
}