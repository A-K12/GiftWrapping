﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GiftWrapping;
using GiftWrapping.Structures;
using NUnit.Framework;

namespace GiftWrappingTest
{
    [TestFixture]
    public class PointTest
    {
        [Test]
        public void Set_IndexOutOfRange_ThrowsException()
        {

            Point point = new Point(new double[3]);

            Exception ex = Assert.Catch<Exception>(() =>
            {
                double i = point[4];
            });

            StringAssert.Contains("Index was outside the bounds of the array", ex.Message);
        }


        [Test]
        public void GetDimension_WhenCall_ReturnDimension()
        {
            int exceptDimension = 555;
            Point point = new Point(new double[exceptDimension]);

            int resultingDimension = point.Dim;

            Assert.AreEqual(exceptDimension, resultingDimension);
        }

        [Test]
        public void Equals_EqualObjects_ReturnTrue()
        {
            Point p1 = new Point(new double[] {4, 4, 0, 0});
            Point p2 = new Point(new double[] {4, 4, 0, 0});

            Assert.AreEqual(p1, p2);
        }


        [Test]
        public void Equals_PointArrays_ReturnTrue()
        {
            Point p1 = new Point(new double[] { 1, 1, 1, 0 });
            Point p2 = new Point(new double[] { 4, 4, 0, 0 });
            Point p3 = new Point(new double[] { 1, 1, 1, 0 });
            Point p4 = new Point(new double[] { 4, 4, 0, 0 });
            Point[] points1 = new Point[] { p1, p2 };
            Point[] points2 = new Point[] { p3, p4 };


            Assert.AreEqual(points1, points2);
        }

      
    }
}