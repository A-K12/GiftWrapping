using System;
using System.Collections.Generic;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane
    {
        public int Dim { get; }

        public Vector Normal { get; private set; }

        public List<Point> Points { get; protected set;  }

        public Hyperplane(Vector normal)
        {
            Dim = normal.Dim;
            Normal = normal;
            Points = new List<Point>();
        }

        public Hyperplane(Point[] points, Vector[] vectors)
        {
            Dim = points[0].Dim;
            Normal = FindNormal(points, vectors);
            this.Points = new List<Point>(points);
        }

        private Vector FindNormal(Point[] points, Vector[] vectors)
        {
            Matrix leftSide = MatrixBuilder.CreateMatrix(points, vectors);
            Vector rightSide = new Vector(Dim);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide).Normalize();
        }

        public Hyperplane(Point[] points)
        {
            if (!HaveSameDimension(points))
            {
                throw new ArgumentException("Points don't have same dimension");
            }
            Dim = (points.Length != 0) 
                ? points[0].Dim
                : throw new ArgumentException("an array of points is empty");
            //if (Math.Abs(points.Length - Dim) > 0)
            //{
            //    throw new ArgumentException("Number of points is not equal to dimension.");
            //}

            Normal = FindNormal(points);
            Points = new List<Point>(points);
        }

        private static bool HaveSameDimension(Point[] points)
        {
            bool result = true;
            for (int i = 1; i < points.Length; i++)
            {
                result &= points[i].Dim == points[0].Dim;
            }

            return result;
        }

        private Vector FindNormal(Point[] points)
        {
            Matrix leftSide = MatrixBuilder.CreateMatrix(points);
            Vector rightSide = new Vector(Dim);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide).Normalize();
        }

        public double Angle(Hyperplane hyperplane)
        {
            return Vector.Angle(this.Normal, hyperplane.Normal);
        }

        public double Cos(Hyperplane hyperplane)
        {
            return (this.Normal * hyperplane.Normal) / this.Normal.Length / hyperplane.Normal.Length;
        }

        public int Side(Point point)
        {
            Vector vector = new Vector(point);
            
            return Tools.CMP(Normal * vector);
        }

        public void ReorientNormal()
        {
            Normal = -Normal;
        }
    }   
}