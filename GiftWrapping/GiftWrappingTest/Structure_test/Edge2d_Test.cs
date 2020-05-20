using System;
using System.Collections.Generic;
using FluentAssertions;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest.Structure_test
{
    public class Edge2d_Test
    {
        public static IEnumerable<TestCaseData> GetTestEdges()
        {
            Point p1 = new Point(new double[] { 1, 1, 1, 0 });
            Point p2 = new Point(new double[] { 4, 4, 0, 0 });
            Point p3 = new Point(new double[] { 1, 1, 1, 0 });
            Point p4 = new Point(new double[] { 4, 4, 0, 0 });
            Point p5 = new Point(new double[] { 1, 1, 1, 1 });

            Edge2d edge1 = new Edge2d(p1, p2);
            Edge2d edge2 = new Edge2d(p3, p4);
            yield return new TestCaseData(edge1, edge2).SetName("Equals_When same point and same order.").Returns(true);

            Edge2d edge3 = new Edge2d(p4, p3);
            yield return new TestCaseData(edge1, edge3).SetName("Equals_When same point and different order.").Returns(true);

            Edge2d edge4 = new Edge2d(p4, p5);
            yield return new TestCaseData(edge1, edge4).SetName("Equals_When different point.").Returns(false);

        }

        [Test, TestCaseSource("GetTestEdges")]
        public bool Equals_WhenCalls(Edge2d point1, Edge2d point2)
        {
            return point1.Equals(point2);
        }

        [Test]
        public void Initialization_WhenSamePoints_ThrowsException()
        {
            Point p1 = new Point(new double[] { 1, 1, 1, 0 });
            Point p2 = new Point(new double[] { 1, 1, 1, 0 });


            Assert.Throws<ArgumentException>((() => new Edge2d(p1, p2)));
        }

        

        [Test]
        public void Initialization_WhenDifferentPoints_CreatesEdge2d()
        {
            Point p1 = new Point(new double[] { 1, 1, 1, 0 });
            Point p2 = new Point(new double[] { 0, 0, 0, 0 });


            Assert.DoesNotThrow((() => new Edge2d(p1, p2)));
        }

        [Test]
        public void Points_ContainsInsertedPoint()
        {
            Point p1 = new Point(new double[] { 1, 1, 1, 0 });
            Point p2 = new Point(new double[] { 0, 0, 0, 0 });
            Point[] points = new[] {p1, p2};

            Edge2d edge = new Edge2d(p1, p2);

            edge.Points.Should().Equal(points);
        }
    }
}