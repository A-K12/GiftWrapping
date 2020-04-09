using System;
using System.ComponentModel.DataAnnotations;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class Temp
    {
        [Test]
        public void SThrowsException()
        {

          int[] test = new int[]{1,2,3};

          int[] test1 = new int[]{3,2,1};

          int h1 = test1.GetHashCode();
          int h2 = test.GetHashCode();


          Assert.AreEqual(h1, h2);
        }


        [Test]
        public void GReturnDimension()
        {
            int exceptDimension = 555;
            Point point = new Point(new double[exceptDimension]);

            int resultingDimension = point.Dim;

            Assert.AreEqual(exceptDimension, resultingDimension);
        }
    }
}