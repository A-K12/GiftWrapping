using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane
    {
        public int Dim { get; protected set; }
        public Point MainPoint { get => _points.First(); }
        public Vector Normal { get; protected set; }
        protected List<Point> _points;
        public IList<Point> Points => _points.AsReadOnly();


        public Hyperplane(Point point, Vector normal)
        {
            Dim = normal.Dim;
            Normal = normal;
            _points = new List<Point>{point};
        }

        public Hyperplane(Point point, Vector[] vectors)
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(vectors));
            }
            Dim = point.Dim;
            if (Dim < vectors.Length)
            {
                throw new ArgumentException("The plane cannot be found . There are not enough vectors.");
            }
            _points = new List<Point> { point };
            Normal = FindNormal(vectors);
        }

        public Hyperplane(Point[] points)
        {
            if (points.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(points));
            }
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Points don't have same dimension");
            }
            this.Dim = points[0].Dim;
            if (points.Length != Dim)
            {
                throw new ArgumentException("Number of points is not equal to dimension.");
            }
            Normal = FindNormal(points);
            _points = new List<Point>(points);
        }


        private Vector FindNormal(Point[] points)
        {
            Vector[] vectors = points.ToVectors();

            return FindNormal(vectors);
        }

        private Vector FindNormal(Vector[] vectors)
        {
            Matrix leftSide = vectors.ToMatrix();
            Vector rightSide = new Vector(Dim);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide).Normalize();
        }

        public double Angle(Hyperplane hyperplane)
        {
            return Vector.Angle(this.Normal, hyperplane.Normal);
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

        public bool TryAddPoint(Point point)
        {
            if (_points.Contains(point)) return false;
            double result = 0;
            for (int i = 0; i < Normal.Dim; i++)
            {
                result += Normal[i] * (point[i] - MainPoint[i]);
            }

            if (Tools.NE(result)) return false;
            _points.Add(point); 

            return true;
        }

        public void TryAddPoints(Point[] points)
        {
            foreach (Point point in points)
            {
                TryAddPoint(point);
            }
        }
    }   
}