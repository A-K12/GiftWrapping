using System;
using System.Linq;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class PlaneFinderTest
    {
        [Test]
        public void FindFirstPlane_WhenCall_ReturnPoints()
        {
            Point[] points = new Point[] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 0, 0}),
                new Point(new double[]{1.5, 1.5, 1}),
                new Point(new double[]{1.4, 1.4, 1}),
                new Point(new double[]{1, 1, 0.5}),
                new Point(new double[]{1.5, 1, 0.4})
            };
            Point[] expect1 = new Point[3] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 0})
            };
            Point[] expect2 = new Point[3] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 4})
            };
            Point[] expect3 = new Point[3] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 0, 0})
            };
            Point[] expect4 = new Point[3] {
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 0})
            };
            PlaneFinder faceFinder = new PlaneFinder(points.ToList());

            Hyperplane result = faceFinder.FindFirstPlane();

            Assert.That(result.Points, Is.EquivalentTo(expect1).Or.EquivalentTo(expect2).
                Or.EquivalentTo(expect3).Or.EquivalentTo(expect4));
        }

        [Test]
        public void FindStartingVector_WhenCall_GetMinimalVector()
        {
            Point[] points = new Point[5] {
                new Point(new double[]{2, 3}),
                new Point(new double[]{3, 2}),
                new Point(new double[]{1, 2}),
                new Point(new double[]{5, 5}),
                new Point(new double[]{2, 2})};

            Point result = PlaneFinder.FindStartingPoint(points.ToList());

            Assert.AreSame(points[2], result);
        }

        [Test]
        public void FindStartingVector_EmptyArray_ThrowsException()
        {
            Point[] points = new Point[0];
            

            Exception ex = Assert.Catch<ArgumentException>(() =>
            {
                PlaneFinder faceFinder = new PlaneFinder(points.ToList());
            });

            StringAssert.Contains("Sequence contains no elements", ex.Message);
        }


    }
}