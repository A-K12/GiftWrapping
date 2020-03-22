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
                MatrixHelper.CreateMatrix(emptyArray);
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
                MatrixHelper.CreateMatrix(points);
            });

            StringAssert.Contains("Sequence contains more elements than point dimension", ex.Message);
        }

        [Test]
        public void ConvertVectorToMatrix_RightVector_ReturnMatrix()
        {
            Vector[] vectors = new Vector[3];
            vectors[0] = new Vector(new double[] { 2, 2, 2 });
            vectors[1] = new Vector(new double[] { 3, 4, 5 });
            vectors[2] = new Vector(new double[] { 5, 6, 7 });
            double[] expect = new double[] {2, 2, 2, 3, 4, 5, 5, 6, 7};
            Matrix expectMatrix = new Matrix(vectors.Length, vectors[0].Dim, expect);
   
            Matrix result = MatrixHelper.ConvertVectorToMatrix(vectors);

            Assert.AreEqual(expectMatrix, result);
        }


    }
}