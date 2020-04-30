using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class PlaneFinderTest
    {
        private static IEnumerable SetPoints()
        {
            Point[] points = new Point[] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 0, 0}),
                new Point(new double[]{0.5, 0.5, 0.5}),
                new Point(new double[]{1, 1, 1}),
                new Point(new double[]{1, 1, 0.5})
            };
            Vector v1 = new Vector(new double[] { 0, 0, -1 });
            Vector v2 = new Vector(new double[] { 0, -1, 0 });
            Vector v3 = new Vector(new double[] { -1, 0, 0 });
            Vector v4 = new Vector(new double[] { -1, -1, -1 });
            Hyperplane[] expect = new Hyperplane[]
            {
                new Hyperplane(points[3], v1),
                new Hyperplane(points[3], v2),
                new Hyperplane(points[3], v3),
                new Hyperplane(points[3], v4)
            };

            yield return new object[] {points, expect};

            points = new Point[] {
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{0.5, 0.5, 0.5, 0.5}),
                new Point(new double[]{1, 1, 1, 0.5}),
                new Point(new double[]{3, 1, 1, 1}),
                new Point(new double[]{1, 3, 1, 1}),
                new Point(new double[]{1, 1, 3, 1}),
                new Point(new double[]{1, 1, 1, 3}),
                new Point(new double[]{1, 1, 1, 1})
            };

            v1 = new Vector(new double[] { 0, 0, 0, -1 });
            v2 = new Vector(new double[] { 0, 0, -1, 0 });
            v3 = new Vector(new double[] { 0, -1, 0, 0 });
            v4 = new Vector(new double[] { -1, 0, 0, 0 });
            Vector v5 = new Vector(new double[] { -1, -1, -1, -1 });
            expect = new Hyperplane[]
            {
                new Hyperplane(points[4], v1),
                new Hyperplane(points[4], v2),
                new Hyperplane(points[4], v3),
                new Hyperplane(points[4], v4),
                new Hyperplane(points[4], v5)
            };

            yield return new object[] { points, expect };

            points = new Point[] {
                new Point(new double[]{4, 0, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0, 0}),
                new Point(new double[]{0, 0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0, 0}),
                new Point(new double[]{0.5, 0.5, 0.5, 0.5, 0}),
                new Point(new double[]{1, 1, 1, 0.5, 0.5}),
                new Point(new double[]{3, 1, 1, 1, 1}),
                new Point(new double[]{1, 3, 1, 1, 1}),
                new Point(new double[]{1, 1, 3, 1, 1}),
                new Point(new double[]{1, 1, 1, 3, 1}),
                new Point(new double[]{1, 1, 1, 1, 3}),
                new Point(new double[]{1, 1, 1, 1, 1})
            };
            v1 = new Vector(new double[] { 0, 0, 0, 0, -1 });
            v2 = new Vector(new double[] { 0, 0, 0, -1, 0 });
            v3 = new Vector(new double[] { 0, 0, -1, 0, 0 });
            v4 = new Vector(new double[] { 0, -1, 0, 0, 0 });
            v5 = new Vector(new double[] { -1, 0, 0, 0, 0 });
            Vector v6 = new Vector(new double[] { -1, -1, -1, -1 });
            expect = new Hyperplane[]
            {
                new Hyperplane(points[5], v1),
                new Hyperplane(points[5], v2),
                new Hyperplane(points[5], v3),
                new Hyperplane(points[5], v4),
                new Hyperplane(points[5], v5)
            };

            yield return new object[] { points, expect };
        }

        [Test, TestCaseSource(nameof(SetPoints))]
        public void FindFirstPlane_Simplex_ReturnPoints(Point[] points, Hyperplane[] expected)
        {
            PlaneFinder planeFinder = new PlaneFinder();

            Hyperplane result = planeFinder.FindFirstPlane(points);

            Assert.True(expected.Contains(result));
        }

        [Test]
        public void FindStartingVector_WhenCall_ReturnIndexVector()
        {
            Point[] points = new Point[5] {
                new Point(new double[]{2, 3}),
                new Point(new double[]{3, 2}),
                new Point(new double[]{1, 2}),
                new Point(new double[]{5, 5}),
                new Point(new double[]{2, 2})};

            Point result = points.FindMinimumPoint();


            Assert.AreEqual(points[2], result);
        }

        [Test]
        public void PlaneFinder_EmptyArray_ThrowsException()
        {
            Point[] points = new Point[0];
            PlaneFinder planeFinder = new PlaneFinder();


            Exception ex = Assert.Catch<ArgumentException>(() =>
            {
             
            });

            StringAssert.Contains("Sequence contains no elements", ex.Message);
        }


    }
}