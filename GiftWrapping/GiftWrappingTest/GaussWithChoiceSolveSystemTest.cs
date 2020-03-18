﻿using System;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace GiftWrappingTest
{
    [TestFixture]
    public class GaussWithChoiceSolveSystemTest
    {
        private static readonly object[] SetLinearEquationsSystems =
        {
            new object[] {new Matrix(2, 3, new double[] {1, 2, 3, 3, 5, 7}), new Vector(new double[] {3, 0})}
        };

        [Test, TestCaseSource(nameof(SetLinearEquationsSystems))]
        public void GetRandomAnswer_AnyMatrix_ReturnVector(Matrix matrix, Vector vector)
        {

            Vector result = GaussWithChoiceSolveSystem.FindAnswer(matrix, vector);
            Vector rightSide = CalculateRightSide(matrix, result);

            Assert.AreEqual(vector, rightSide);
        }


        private Vector CalculateRightSide(Matrix matrix, Vector variables)
        {
            double[] rigtSide = new double[matrix.Rows];
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    rigtSide[i] += matrix[i, j] * variables[j];
                }
            }

            return new Vector(rigtSide);
        }

        [Test]
        public void GetRandomAnswer_DeterminateMatrix_ReturnVector()
        {
            double[,] points = new double[3, 3]
            {
                {1, 2, 3},
                {3, 5, 7},
                {1, 3, 4}
            };
            Matrix matrix = new Matrix(points);
            Vector vector = new Vector(new double[3] {3, 0, 1});
            Vector expect = new Vector(new double[3] {-4, -13, 11});


            Vector result = GaussWithChoiceSolveSystem.FindAnswer(matrix, vector);

            Assert.AreEqual(expect, result);
        }
    }
}