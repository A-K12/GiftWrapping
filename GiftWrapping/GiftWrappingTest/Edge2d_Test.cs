using System.Collections.Generic;
using FluentAssertions;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class Edge2d_Test
    {

        [Test]
        public void Equals_WhenSamePoints_ReturnTrue()
        {
            Point p1 = new Point(new double[] { 1, 1, 1, 0 });
            Point p2 = new Point(new double[] { 4, 4, 0, 0 });
            Point p3 = new Point(new double[] { 1, 1, 1, 0 });
            Point p4 = new Point(new double[] { 4, 4, 0, 0 });
            Point[] points1 = new Point[] { p1, p2 };
            Point[] points2 = new Point[] { p4, p3 };

            Edge2d ch1 = new Edge2d();
            ch1.Points[0] = p1;
            ch1.Points[1] = p2;


            Edge2d ch2 = new Edge2d();
            ch2.Points[0] = p4;
            ch2.Points[1] = p3;

            ch1.Should().Equals(ch2);
        }

        [Test]
        public void q444uals_WhenSamePoints_ReturnTrue()
        {
            Vector p1 = new Vector(new double[] { -1, 0});
            Vector p2 = new Point(new double[] { -1, -1});

            double first = Vector.Angle(p1, p2);
            double first1 = Vector.Angle(p2, p1);


        }


    }
}