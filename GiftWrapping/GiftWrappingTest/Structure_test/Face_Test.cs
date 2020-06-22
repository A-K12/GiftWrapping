using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest.Structure_test
{
    [TestFixture]
    public class Face_Test
    {
        public static IEnumerable<TestCaseData> GetTestFaces()
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

            Face f1 = new Face(3) {ConvexHull = ch1};
            Face f2 = new Face(3) {ConvexHull = ch2};
            yield return new TestCaseData(f1, f2).SetName("{m}_When same point but different order.").Returns(true);

            Face f3 = new Face(3) { ConvexHull = ch1 };
            Face f4 = new Face(3) { ConvexHull = ch3 };
            yield return new TestCaseData(f3, f4).SetName("{m}_Equals_When same point and same order.").Returns(true);

            Face f5 = new Face(3) { ConvexHull = ch4 };
            Face f6 = new Face(3) { ConvexHull = ch1 };
            yield return new TestCaseData(f5, f6).SetName("{m}_Equals_When different point.").Returns(false);
        }

        [Test, TestCaseSource("GetTestFaces")]
        public bool Equals_WhenCalls(Face point1, Face point2)
        {
            return point1.Equals(point2);
        }

        [Test]
        public void Initialization_WhenPlaneNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>((() => new Face(null)));
        }

        [Test]
        public void Initialization_UsingHyperplane()
        {
            Hyperplane plane1 = new Hyperplane(new PlanePoint(new double[] {0, 0}), new Vector(new double[] {1, 1}));
            Face face = new Face(plane1);

            face.Hyperplane.Should().Equals(plane1);
        }

        [Test]
        public void Initialization_UsingDimension()
        {
            int dim = 100;

            Face face = new Face(dim);

            face.Dimension.Should().Equals(dim);
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
            Face face = new Face(2);
            face.ConvexHull = ch;

            List<PlanePoint> points = face.GetPoints().ToList();

            points.Should().Equal(p1);
        }
    }
}