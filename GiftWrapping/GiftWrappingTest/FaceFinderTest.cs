using System;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class FaceFinderTest
    {
        [Test]
        public void test_WhenCall_GetMinimalPoint()
        {
            var points = new Point[4] {
                new Point(1, 1,0),
                new Point(6, 1,0),
                new Point(3,3,0),
                new Point(3,3,6)
            };
            var expect = new Point[3] {
                new Point(1, 1,0),
                new Point(3,3,0),
                new Point(3,3,6)
            };

            var faceFinder = new FaceFinder();

            var result = faceFinder.FindFacePoints(points);

            Assert.AreEqual(expect, result);
            //Assert.AreEqual(expect[1], result[1]);
            //Assert.AreEqual(expect[2], result[2]);

        }

        [Test]
        public void FindStartingPoint_WhenCall_GetMinimalPoint()
        {
            var points = new Point[2] {
                new Point(1, 5), 
                new Point(2, 2)};
            var faceFinder = new FaceFinder();

            var result = faceFinder.FindStartingPoint(points);

            Assert.AreSame(points[0], result);
        }

        [Test]
        public void FindStartingPoint_EmptyArray_ThrowsException()
        {
            var points = new Point[0];
            var faceFinder = new FaceFinder();

            var ex = Assert.Catch<Exception>(() =>
            {
                var i = faceFinder.FindStartingPoint(points);
            });

            StringAssert.Contains("Sequence contains no elements", ex.Message);
        }


    }
}