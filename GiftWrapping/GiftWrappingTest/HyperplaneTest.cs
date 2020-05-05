using System;
using System.Linq;
using GiftWrapping;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    public class HyperplaneTest
    {

        [Test]
        public void Create_2dPoints_ReturnHyperplane()
        {
            Point[] points = new Point[]
            {
                new Point(new double[] {0, 0}),
                new Point(new double[] {0, 4})
            };
            Vector normal = new Vector(new double[]{-1, 0});
            Hyperplane h2 = new Hyperplane(points[0], normal);

            Hyperplane h = HyperplaneHelper.Create(points, new IndexMap(2));

            Assert.AreEqual(h2,h);
        }

        [Test]
        public void Angle_WhenCall_ReturnAngle()
        {
            Point p1 = new Point(3);
            Vector n1 = new Vector(new double[]{1,0,0});
            Vector n2 = new Vector(new double[] { 0, 1, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Hyperplane h2 = new Hyperplane(p1, n2);
            const double expect = Math.PI/2;

            double result = h1.Angle(h2);

            Assert.AreEqual(expect, result, Constants.Esp);
        }

        [Test]
        public void Angle_SamePlane_ReturnAngle()
        {
            Point p1 = new Point(3);
            Vector n1 = new Vector(new double[] { 1, 0, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            const double expect = 0;

            double result = h1.Angle(h1);

            Assert.AreEqual(expect, result, Constants.Esp);
        }



        [Test]
        public void Side_PositivePoint_ReturnPosition()
        {
            Point p1 = new Point(3);
            Vector n1 = new Vector(new double[] { 1, 0, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Point p2 = new Point(new double[]{1,1,1});
            const int expect = 1;

            int result = h1.Side(p2);

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void Side_NegativePoint_ReturnPosition()
        {
            Point p1 = new Point(3);
            Vector n1 = new Vector(new double[] { 1, 0, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Point p2 = new Point(new double[] { -1, -1, -1 });
            const int expect = -1;

            int result = h1.Side(p2);

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void Side_PointOfPlane_ReturnPosition()
        {
            Point p1 = new Point(3);
            Vector n1 = new Vector(new double[] { 1, 0, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Point p2 = new Point(new double[] { 0, 4, 4 });
            const int expect = 0;

            int result = h1.Side(p2);

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void ReorientNormal_WhenCall_ChangeOrientationNormal()
        {
            Point p1 = new Point(3);
            Vector n1 = new Vector(new double[] { 1, 1, 1 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Vector n2 = new Vector(new double[] { -1, -1, -1 });
            Hyperplane h2 = new Hyperplane(p1, n2);

            h1.ReorientNormal();

            Assert.AreEqual(h2.Normal, h1.Normal);
        }

        [Test]
        public void Equals_EqualPlane_ReturnTrue()
        {
            Point p1 = new Point(new double[]{1, 7, 0});
            Vector n1 = new Vector(new double[] { 2, -1, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Point p2 = new Point(new double[] { -1, 3, 0 });
            Vector n2 = new Vector(new double[] { -4, 2, 0 });
            Hyperplane h2 = new Hyperplane(p2, n2);

             bool result = h1.Equals(h2);

            Assert.AreEqual(true, result);
        }


        [Test]
        public void Equals_UnequalPlane_ReturnFalse()
        {
            Point p1 = new Point(new double[] { 1, 7, 0 });
            Vector n1 = new Vector(new double[] { 1, -1, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Point p2 = new Point(new double[] { -1, 3, 0 });
            Vector n2 = new Vector(new double[] { -4, 2, 0 });
            Hyperplane h2 = new Hyperplane(p2, n2);

            bool result = h1.Equals(h2);

            Assert.AreEqual(false, result);
        }


        [Test]
        public void GetHashCode_WhenCall_SameValue()
        {
            Point p1 = new Point(new double[] { 1, 7, 0 });
            Vector n1 = new Vector(new double[] { -2, 1, 0 });
            Hyperplane h1 = new Hyperplane(p1, n1);
            Point p2 = new Point(new double[] { -1, 3, 0 });
            Vector n2 = new Vector(new double[] { -4, 2, 0 });
            Hyperplane h2 = new Hyperplane(p2, n2);

            int result1 = h1.GetHashCode();
            int result2 = h2.GetHashCode();
            
            Assert.AreEqual(result1,result2 );
        }
    }
}