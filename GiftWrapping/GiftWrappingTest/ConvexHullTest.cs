using System;
using System.Collections.Generic;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class ConvexHullTest
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
            ConvexHull2d c1 = new ConvexHull2d();
            c1.Points = p1;
            ConvexHull2d c2 = new ConvexHull2d();
            c2.Points = p2;
            ConvexHull2d c3 = new ConvexHull2d();
            c3.Points = p3;
            ConvexHull2d c4 = new ConvexHull2d();
            c4.Points = p4;
            
            ConvexHull ch1 = new ConvexHull(3);
            ch1.InnerFaces =new List<ICell>{c1,c2};

            ConvexHull ch2 = new ConvexHull(3);
            ch2.InnerFaces =new List<ICell>{c3,c4};


            Assert.AreEqual(ch1, ch2);
        }
    }
}