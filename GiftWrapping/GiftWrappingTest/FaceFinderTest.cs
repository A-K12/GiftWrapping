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
        public void test_WhenCall_GetMinimalVector()
        {
            var Vectors = new Vector[4] {
                new Vector(new double[]{1, 1,0 }),
                new Vector(new double[]{6, 1,0}),
                new Vector(new double[]{3,3,0}),
                new Vector(new double[]{3,3,6})
            };
            var expect = new Vector[3] {
                new Vector(new double[]{1, 1,0}),
                new Vector(new double[]{3,3,0}),
                new Vector(new double[]{3,3,6})
            };

            var faceFinder = new FaceFinder();

            var result = faceFinder.FindFacePoints(Vectors);

            Assert.AreEqual(expect, result);
            //Assert.AreEqual(expect[1], result[1]);
            //Assert.AreEqual(expect[2], result[2]);

        }

        [Test]
        public void FindStartingVector_WhenCall_GetMinimalVector()
        {
            //var Vectors = new Vector[2] {
            //    new Vector(1, 5), 
            //    new Vector(2, 2)};
            //var faceFinder = new FaceFinder();

            //var result = faceFinder.FindFacePoints(Vectors);

            //Assert.AreSame(Vectors[0], result);
        }

        [Test]
        public void FindStartingVector_EmptyArray_ThrowsException()
        {
            var Vectors = new Vector[0];
            var faceFinder = new FaceFinder();

            var ex = Assert.Catch<Exception>(() =>
            {
                var i = faceFinder.FindFacePoints(Vectors);
            });

            StringAssert.Contains("Sequence contains no elements", ex.Message);
        }


    }
}