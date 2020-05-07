using System.Collections.Generic;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class ConvexHull2dTest
    {
        [Test]
        public void Equals_EqualObjects_ReturnTrue()
        {
            List<Point> points1 = new List<Point> {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{4, 4, 0, 0}),
            };
            List<Point> points2 = new List<Point> {
                new Point(new double[]{0, 0, 0, 0}),
                new Point(new double[]{4, 0, 0, 0}),
                new Point(new double[]{0, 4, 0, 0}),
                new Point(new double[]{4, 4, 0, 0}),
            };
            ConvexHull2d first = new ConvexHull2d();
            first.Points = points1;
            ConvexHull2d second = new ConvexHull2d();
            second.Points = points2;

            Assert.AreEqual(first, second);
        }
    }
}