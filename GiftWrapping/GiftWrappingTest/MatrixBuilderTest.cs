using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class MatrixBuilderTest
    {

        [Test]
        public void CreateMatrix_EmptyArray_ThrowException()
        {
            Point[] emptyArray = new Point[0];

            Exception ex = Assert.Catch<Exception>(() =>
            {
                MatrixBuilder.CreateMatrix(emptyArray);
            });

            StringAssert.Contains("Sequence contains less than two elements", ex.Message);
        }

        [Test]
        public void CreateMatrix_BigArray_ThrowException()
        {
            Point[] points = new Point[4];
            points[0] = new Point(new double[] { 2, 2, 2});
            points[1] = new Point(new double[] { 3, 4, 5});
            points[2] = new Point(new double[] { 5, 6, 7});
            points[3] = new Point(new double[] { 5, 6, 7});


            Exception ex = Assert.Catch<Exception>(() =>
            {
                MatrixBuilder.CreateMatrix(points);
            });

            StringAssert.Contains("Sequence contains more elements than point dimension", ex.Message);
        }

        [Test]
        public void CreateMatrix_UsualArray_ThrowException()
        {
            Point[] points = new Point[3];
            points[0] = new Point(new double[] { 2, 2, 2, 2, 2 });
            points[1] = new Point(new double[] { 3, 4, 5, 6, 7 });
            points[2] = new Point(new double[] { 5, 6, 7, 8, 9 });
            double[,] expect = new double[4, 5]
            {
                {1, 2, 3, 4, 5}, 
                {3, 4, 5, 6, 7}, 
                {0, 0, 0, 1, 0}, 
                {0, 0, 0, 0, 1}
            };
            Matrix expectMatrix = new Matrix(expect);

            Matrix matrix = MatrixBuilder.CreateMatrix(points);

            Assert.AreEqual(expectMatrix, matrix);
        }
    }
}