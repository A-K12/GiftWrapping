using System;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class LinearEquationsSolverTest
    {
        [Test]
        public void GaussWithChoiceSolveSystem_DeterminateMatrix_ReturnVector()
        {
            var points = new double[3,3]
            {
                {1, 2, 3}, 
                {3, 5, 7},
                {1, 3, 4}
            };
            var matrix = new Matrix(points);
            var vector = new Vector(new double[3]{ 3, 0, 1 });
            var expect = new Vector(new double[3]{ -4, -13, 11 });
            var solver = new LinearEquationsSolver();


            var result = solver.GaussWithChoiceSolveSystem(matrix, vector);

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void GaussWithChoiceSolveSystem_AnyMatrix_ReturnVector() //Matrix matrix, Vector vector
        {
            var points = new double[2, 3]
            {
                {1, 2, 3},
                {3, 5, 7}
            };
            var matrix = new Matrix(points);
            var vector = new Vector(new double[2] { 3, 0});
           // var expect = new Vector(new double[3] {-11, 1, 4});
            var solver = new LinearEquationsSolver();

            var result = solver.GaussWithChoiceSolveSystem(matrix, vector);
            var rigthSide = CalculateRightSide(matrix, result);

            Assert.AreEqual(vector, rigthSide);
        }


        private Vector CalculateRightSide(Matrix matrix, Vector variables)
        {
            var rigtSide = new double[matrix.Rows];
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    rigtSide[i] += matrix[i, j] * variables[j];
                }
            }

            return new Vector(rigtSide);
        }
    }
}