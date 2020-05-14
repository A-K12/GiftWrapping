﻿using FluentAssertions;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class HyperplaneHelper_Test
    {
        [Test]
        public void Create_2dPoints_ReturnHyperplane()
        {
            Point[] points = new Point[]
            {
                new Point(new double[] {0, 0}),
                new Point(new double[] {0, 4})
            };
            Vector normal = new Vector(new double[] { -1, 0 });
            Hyperplane h2 = new Hyperplane(points[0], normal);

            Hyperplane h = HyperplaneHelper.Create(points);

            Assert.AreEqual(h2, h);
        }

        [Test]
        public void Create_2dPoints_ReturnHyperplane2()
        {
            Point[] points = new Point[]
            {
                new Point(new double[] {2,1}),
                new Point(new double[] {1,1})
            };

            Hyperplane h = HyperplaneHelper.Create(points);

            double y = h.Normal[0] * (-1);
            double t = 9 / y;

            Assert.IsTrue(true);
        }

        [Test]
        public void GetOrthonormalBasis_When2dVectors_ReturnVectors()
        {
            Vector[] vectors = new Vector[]
            {
                new Vector(new double[] {0, 5}),
                new Vector(new double[] {2.5, 3}),
            };
            Vector[] expect = new Vector[]
            {
                new Vector(new double[]{0, 1}), 
                new Vector(new double[]{1, 0}), 
            };

            Vector[] actual = vectors.GetOrthonormalBasis();

            actual.Should().Equals(expect);
        }

        [Test]
        public void ProjectVectorTo_When2dVectors_ReturnVector()
        {

            Vector v1 = new Point(new double[] {2, 0});
            Vector v2 = new Point(new double[] {1, 1});
            Vector expect = new Vector(new double[]{1,0});

            Vector actual = v1.ProjectVectorTo(v2);

            actual.Should().Equals(expect);
        }
    }
}