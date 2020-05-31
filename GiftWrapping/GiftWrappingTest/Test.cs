using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestTest()
        {
            double d1 = 2.0000001;

            double d2 = 4.0 / d1;

            double d3 = 4.00000002 / d1;

            double test = 4.9;

            int test2 = (int) test;
            
            double d4 = d2 * 1000000;
            double d5 = d3 * 1000000;

            int d6 = (int) d4;
            int d7 = (int)d5;

            
            int r = d2.GetHashCode();

            int p = d3.GetHashCode();
            
            int r2 = d7.GetHashCode();

            int p2 = d6.GetHashCode();

            Dictionary<double, int> rrr = new Dictionary<double, int>();

            rrr[d2] = 1;
            rrr[d3] = 3;
            rrr[d6] = 1000;
            rrr[d7] = 10;


            Assert.AreEqual(r,p);
        }

    }
}