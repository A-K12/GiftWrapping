using System;
using System.Collections.Generic;
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

            Point point = new Point(new double[3]);

            Exception ex = Assert.Catch<Exception>(() =>
            {
                double i = point[4];
            });

            StringAssert.Contains("Index was outside the bounds of the array", ex.Message);
        }


        [Test]
        public void GetDimension_WhenCall_ReturnDimension()
        {
            int exceptDimension = 555;
            Point point = new Point(new double[exceptDimension]);

            int resultingDimension = point.Dim;

            Assert.AreEqual(exceptDimension, resultingDimension);
        }

        [Test]
        public void Equals_EqualObjects_ReturnTrue()
        {
            Point p1 = new Point(new double[] {4, 4, 0, 0});
            Point p2 = new Point(new double[] {4, 4, 0, 0});

            Assert.AreEqual(p1, p2);
        }
    }
}