using System;
using System.ComponentModel.DataAnnotations;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class PointTest
    {
        [Test]
        public void Set_IndexOutOfRange_ThrowsException()
        {
            var point = new Point(new double[3]);

            var ex = Assert.Catch<Exception>(() =>
            {
                var i = point[4];
            });

            StringAssert.Contains("Index was outside the bounds of the array", ex.Message);
        }


        [Test]
        public void GetDimension_WhenCall_ReturnDimension()
        {
            var exceptDimension = 555;
            var point = new Point(new double[exceptDimension]);

            var resultingDimension = point.Dim;

            Assert.AreEqual(exceptDimension, resultingDimension);
        }
    }
}