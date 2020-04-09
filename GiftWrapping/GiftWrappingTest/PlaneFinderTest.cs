using System;
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
        [Test]
        public void FindFirstPlane_3DSimplex_ReturnPoints()
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
            PlaneFinder planeFinder = new PlaneFinder(points);

            ConvexHull result = planeFinder.FindFirstPlane();

            Assert.That(result.Points, Is.EquivalentTo(expect1).Or.EquivalentTo(expect2).
                Or.EquivalentTo(expect3).Or.EquivalentTo(expect4));
        }

        [Test]
        public void FindFirstPlane_4DSimplex_ReturnPoints()
        {
            Point[] points = new Point[] {
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


            Point[] expect1 = new Point[] {
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 4})
            };
            Point[] expect2 = new Point[] {
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0})
            };
            Point[] expect3 = new Point[] {
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0})
            };
            Point[] expect4 = new Point[] {
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0})
            };
            Point[] expect5 = new Point[] {
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 0})
            };
            PlaneFinder planeFinder = new PlaneFinder(points);

            ConvexHull result = planeFinder.FindFirstPlane();

            Assert.That(result.Points, Is.EquivalentTo(expect1).Or.EquivalentTo(expect2).
                Or.EquivalentTo(expect3).Or.EquivalentTo(expect4).Or.EquivalentTo(expect5));
        }

        [Test]
        public void FindFirstPlane_5DSimplex_ReturnPoints()
        {
            Point[] points = new Point[] {
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


            Point[][] expect = new Point[5][];
            expect[0] = new Point[]
            {
                new Point(new double[] {4, 0, 0, 0, 0}),
                new Point(new double[] {0, 4, 0, 0, 0}),
                new Point(new double[] {0, 0, 4, 0, 0}),
                new Point(new double[] {0, 0, 0, 4, 0}),
                new Point(new double[] {0, 0, 0, 0, 4})
            };
            expect[1] = new Point[] {
                new Point(new double[]{0, 4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0, 0}),
                new Point(new double[]{0, 0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0, 0})
            };
            expect[2] = new Point[] {
                new Point(new double[]{4, 0, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0, 0}),
                new Point(new double[]{0, 0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0, 0})
            };
            expect[3]= new Point[] {
                new Point(new double[]{4, 0, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0, 0}),
                new Point(new double[]{0, 0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 0, 4}),
                new Point(new double[]{0, 0, 0, 0, 0})
            };
            expect[4] = new Point[] {
                new Point(new double[]{4, 0, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0, 0}),
                new Point(new double[]{0, 0, 0, 4, 0}),
                new Point(new double[]{0, 0, 0, 0, 0})
            };


            PlaneFinder planeFinder = new PlaneFinder(points);
            ConvexHull result = planeFinder.FindFirstPlane();

            Assert.That(result.Points, Is.EquivalentTo(expect[0]).Or.EquivalentTo(expect[1]).
                Or.EquivalentTo(expect[2]).Or.EquivalentTo(expect[3]));
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
           

            Exception ex = Assert.Catch<ArgumentException>(() =>
            {
                PlaneFinder planeFinder = new PlaneFinder(points);
            });

            StringAssert.Contains("Sequence contains no elements", ex.Message);
        }


    }
}