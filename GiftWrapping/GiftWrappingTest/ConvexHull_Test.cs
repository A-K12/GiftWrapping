using System;
using System.Collections.Generic;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class ConvexHull_Test
    {
        [Test]
        public void Equals_EqualObjects_ReturnTrue()
        {
            List<Point> p1 = new List<Point> {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
            };
            List<Point> p2 = new List<Point> {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
            };

            List<Point> p3 = new List<Point> {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
            };
            List<Point> p4 = new List<Point> {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 0, 4, 0}),
            };
            ConvexHull c1 = p1.ToConvexHull2d();
            ConvexHull c2 = p2.ToConvexHull2d();
            ConvexHull c3 = p3.ToConvexHull2d();
            ConvexHull c4 = p4.ToConvexHull2d();

            ConvexHull ch1 = new ConvexHull(3);
            ch1.InnerFaces.AddRange(new []{c1, c2});

            ConvexHull ch2 = new ConvexHull(3);
            ch2.InnerFaces.AddRange(new[] { c3, c4 });

            Assert.AreEqual(ch1, ch2);
        }
    }
}