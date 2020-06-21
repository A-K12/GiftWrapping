using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest.Structure_test
{
    [TestFixture]
    public class ConvexHull_Test
    {
        Hyperplane hyperplane = new Hyperplane(new PlanePoint(new double[]{0,0}), new Vector(new double[] { 1, 1 }));
        [Test]
        public void Equals_EqualObjects_ReturnTrue()
        {
         
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
            };
            List<PlanePoint> p2 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
            };

            List<PlanePoint> p3 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
            };
            List<PlanePoint> p4 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
            };
           
            ConvexHull2d c1 = p1.ToConvexHull2d();
            ConvexHull2d c2 = p2.ToConvexHull2d();
            ConvexHull2d c3 = p3.ToConvexHull2d();
            ConvexHull2d c4 = p4.ToConvexHull2d();
            Face f1 = new Face(hyperplane);
            f1.ConvexHull = c1;
            Face f2 = new Face(hyperplane);
            f2.ConvexHull = c2;
            Face f3 = new Face(hyperplane);
            f3.ConvexHull = c3;
            Face f4 = new Face(hyperplane);
            f4.ConvexHull = c4;

            ConvexHull first = new ConvexHull(3);
            first.Cells.Add(f1);
            first.Cells.Add(f2);
            ConvexHull second = new ConvexHull(3);
            second.Cells.Add(f3);
            second.Cells.Add(f4);


            first.Should().Equals(second);
        }


        [Test]
        public void GetPoints_When3dConvexHull_ReturnTrue()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
            };
            List<PlanePoint> p2 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{4, 4}),
            };
            ConvexHull2d c1 = p1.ToConvexHull2d();
            Face f1 = new Face(hyperplane);
            f1.ConvexHull = c1;
            ConvexHull2d c2 = p2.ToConvexHull2d();
            Face f2 = new Face(hyperplane);
            f2.ConvexHull = c2;
            ConvexHull ch1 = new ConvexHull(3);
            ch1.Cells.Add(f1);
            ch1.Cells.Add(f2);
            PlanePoint[] expect = new PlanePoint[]{
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
            };
            Array.Sort(expect);
            
            Point[] actual = ch1.GetPoints().ToArray();
            Array.Sort(actual);
            
            Assert.AreEqual(expect, actual);
        }
    }
}