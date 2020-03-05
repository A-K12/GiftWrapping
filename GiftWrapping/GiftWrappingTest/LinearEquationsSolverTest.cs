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
                {1, 3, 4 }
            };
            var matrix = new Matrix(points);
            var vector = new Vector(new double[3]{ 3, 0, 1 });
            var expect = new Vector(new double[3]{ -4, -13, 11 });
            var solver = new LinearEquationsSolver();


            var result = solver.GaussWithChoiceSolveSystem(matrix, vector);

            Assert.AreEqual(expect, result);
        }

        [Test]
        public void GaussWithChoiceSolveSystem_UndeterminateMatrix_ReturnVector()
        {
            var points = new double[2, 3]
            {
                {1, 2, 3},
                {3, 5, 7}
            };
            var matrix = new Matrix(points);
            var vector = new Vector(new double[2] { 3, 0});
            var expect = new Vector(new double[3] {-11, 1, 4});
            var solver = new LinearEquationsSolver();

            var result = solver.GaussWithChoiceSolveSystem(matrix, vector);

            Assert.AreEqual(expect, result);
        }
    }
}