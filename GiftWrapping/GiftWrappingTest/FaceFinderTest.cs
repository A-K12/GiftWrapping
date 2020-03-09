using System;
using System.Collections.Generic;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class FaceFinderTest
    {
        [Test]
        public void FindFacePoints_WhenCall_ReturnPoints()
        {
            var Points = new Point[] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 0, 0}),
                new Point(new double[]{1.5, 1.5, 1}),
                new Point(new double[]{2, 2, 1.5})
            };
            var expect1 = new Point[3] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 0})
            };
            var expect2 = new Point[3] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 4})
            };
            var expect3 = new Point[3] {
                new Point(new double[]{4, 0, 0}),
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 0, 0})
            };
            var expect4 = new Point[3] {
                new Point(new double[]{0, 0, 4}),
                new Point(new double[]{0, 4, 0}),
                new Point(new double[]{0, 0, 0})
            };
            var faceFinder = new FaceFinder();

            var result = faceFinder.FindFacePoints(Points);

            Assert.That(result, Is.EquivalentTo(expect1).Or.EquivalentTo(expect2).
                Or.EquivalentTo(expect3).Or.EquivalentTo(expect4));
        }

        [Test]
        public void FindStartingVector_WhenCall_GetMinimalVector()
        {
            var Vectors = new Point[5] {
                new Point(new double[]{2, 3}),
                new Point(new double[]{3, 2}),
                new Point(new double[]{1, 2}),
                new Point(new double[]{5, 5}),
                new Point(new double[]{2, 2})};

            var faceFinder = new FaceFinder();

            var result = faceFinder.FindStartingPoint(Vectors);

            Assert.AreSame(Vectors[2], result);
        }

        [Test]
        public void FindStartingVector_EmptyArray_ThrowsException()
        {
            var Vectors = new Point[0];
            var faceFinder = new FaceFinder();

            var ex = Assert.Catch<Exception>(() =>
            {
                var i = faceFinder.FindFacePoints(Vectors);
            });

            StringAssert.Contains("Sequence contains no elements", ex.Message);
        }


    }
}