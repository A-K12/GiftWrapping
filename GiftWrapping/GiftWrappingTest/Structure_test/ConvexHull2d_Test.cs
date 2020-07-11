using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace GiftWrappingTest.Structure_test
{
    [TestFixture]
    public class ConvexHull2d_Test
    {
        


        [Test, TestCaseSource("GetTestHulls")]
        public bool Equals_WhenCalls(ConvexHull2d ch1, ConvexHull2d ch2)
        {

            return ch1.Equals(ch2);
        }

        public static IEnumerable<TestCaseData> GetTestHulls()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
            };
            List<PlanePoint> p1Shuffle = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
            };
            List<PlanePoint> p3 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{1, 1}),
                new PlanePoint(new double[]{2, 2}),
                new PlanePoint(new double[]{0, 2}),
            };

            ConvexHull2d ch1 = new ConvexHull2d(p1);
            ConvexHull2d ch2 = new ConvexHull2d(p1Shuffle);
            ConvexHull2d ch3 = new ConvexHull2d(p1);
            ConvexHull2d ch4 = new ConvexHull2d(p3);

            yield return new TestCaseData(ch1, ch2).SetName("{m}_When same point but different order.").Returns(true);

            yield return new TestCaseData(ch1, ch3).SetName("{m}_Equals_When same point and same order.").Returns(true);

            yield return new TestCaseData(ch1, ch4).SetName("{m}_Equals_When different point.").Returns(false);
        }

        [Test, TestCaseSource("GetIncorrectPoints")]
        public void Initialization_WhenPointsLessThanThree_ThrowException(List<PlanePoint> p)
        {
            Assert.Throws<ArgumentException>(()=> new ConvexHull2d(p));
        }

        private static IEnumerable<TestCaseData> GetIncorrectPoints()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
            };

            List<PlanePoint> p2 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{0, 1}),
            };
            List<PlanePoint> p3 = new List<PlanePoint> {
               
            };

            yield return new TestCaseData(p1).SetName("{m}_When one point.");
            yield return new TestCaseData(p2).SetName("{m}_When two points.");
            yield return new TestCaseData(p3).SetName("{m}_When zero points.");
        }

        [Test]
        public void Initialization_WhenNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(()=> new ConvexHull2d(null));
        }

        [Test]
        public void Initialization_WhenIdenticalPoints_ThrowException()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 4}),
            };

            Assert.Throws<ArgumentException>(() => new ConvexHull2d(p1));
        }

        [Test]
        public void GetPoints_ContainsInsertedPoint()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
            };
            ConvexHull2d ch = new ConvexHull2d(p1);

            List<PlanePoint> points = ch.GetPoints().ToList();

            points.Should().Equal(p1);
        }


        [Test]
        public void Initialization_ContainsEdges()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{0, 4}),
                new PlanePoint(new double[]{4, 4}),
            };
            List<Edge> edges = new List<Edge>()
            {
                new Edge(p1[^1], p1[0]),
                new Edge(p1[0], p1[1]),
                new Edge(p1[1], p1[2]),
                new Edge(p1[2], p1[3]),
            };

            ConvexHull2d ch = new ConvexHull2d(p1);
            List<ICell> result = ch.Cells;

            result.Should().Equal(edges);
        }


        [Test]
        public void Initialization_NormalVectorsOrientedOutward()
        {
            List<PlanePoint> p1 = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0}),
                new PlanePoint(new double[]{4, 0}),
                new PlanePoint(new double[]{4, 4}),
                new PlanePoint(new double[]{0, 4}),
            };
            List<Vector> normals = new List<Vector>()
            {
                new Vector(new double[]{-1, 0}),
                new Vector(new double[]{0, -1}),
                new Vector(new double[]{1, 0}),
                new Vector(new double[]{0, 1}),
            };

            ConvexHull2d ch = new ConvexHull2d(p1);
            List<Vector> result = ch.Cells.Select(cell => cell.Hyperplane.Normal).ToList();

            result.Should().Equal(normals);
        }
    }
}