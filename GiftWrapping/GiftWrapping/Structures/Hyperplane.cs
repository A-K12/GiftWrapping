using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane: IEquatable<Hyperplane>
    {
        public int Dim { get; }
        public Point MainPoint { get => _points.First(); }
        public Vector Normal { get; protected set; }
        
        public double D { get; protected set; }

        protected readonly List<Point> _points;
        public IList<Point> Points => _points.AsReadOnly();


        public Hyperplane(Hyperplane hyperplane)
        {
            Dim = hyperplane.Dim;
            Normal = hyperplane.Normal;
            _points = new List<Point>(hyperplane._points);
            D = CalculateD();
            Normalize();
        }
        public Hyperplane(Point point, Vector normal)
        {
            Dim = normal.Dim;
            Normal = normal;
            _points = new List<Point>{point};
            D = CalculateD();
            Normalize();

        }

        private double CalculateD()
        {
            double d = 0;

            for (int i = 0; i < Normal.Dim; i++)
            {
                d -= Normal[i] * MainPoint[i];
            }

            return d;
        }

        public Hyperplane(Point point, Matrix matrix)
        {
            Dim = point.Dim;
            if (Dim-1 > matrix.Rows)
            {
                throw new ArgumentException("The plane cannot be found . There are not enough matrix.");
            }
            _points = new List<Point> { point };
            Normal = FindNormal(matrix);
            D = CalculateD();
            Normalize();
        }

        public Hyperplane(Point point, Vector[] vectors)
        {
            if (vectors.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(vectors));
            }
            Dim = point.Dim;
            if (Dim-1 > vectors.Length)
            {
                throw new ArgumentException("The plane cannot be found . There are not enough matrix.");
            }
            _points = new List<Point> { point };
            Normal = FindNormal(vectors);
            D = CalculateD();
            Normalize();

        }

        public Hyperplane(IList<Point> points)
        {
            if (points.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(points));
            }
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Points don't have same dimension");
            }
            this.Dim = points[0].Dim;
            if (points.Count != Dim)
            {
                throw new ArgumentException("Number of points is not equal to dimension.");
            }
            Normal = FindNormal(points);
            _points = new List<Point>(points);
            D = CalculateD();
            Normalize();

        }

        private void Normalize()
        {
            D /= Normal.Length;
            Normal = Normal.Normalize();
        }

        private Vector FindNormal(IList<Point> points)
        {
            Vector[] vectors = points.ToVectors();

            return FindNormal(vectors);
        }

        private Vector FindNormal(IList<Vector> vectors)
        {
            Matrix leftSide = vectors.ToMatrix();
            
            return FindNormal(leftSide);
        }

        private Vector FindNormal(Matrix leftSide)
        {
            Vector rightSide = new Vector(leftSide.Rows);

            return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
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

        public bool Equals(Hyperplane other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Dim != other.Dim) return false;
            List<double> quotients = new List<double>();
            for (int i = 0; i < other.Dim; i++)
            {
                if (Tools.EQ(this.Normal[i])&& Tools.EQ(other.Normal[i]))
                {
                    continue;
                }
                quotients.Add(other.Normal[i] / this.Normal[i]);
            }

            if (Tools.NE(this.D) && Tools.NE(other.D))
            {
                quotients.Add(other.D / this.D);
            }

            return quotients.Count == 0 || quotients.All(d => Tools.EQ(d,quotients[0]));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Hyperplane) obj);
        }

        public override int GetHashCode() 
        {
            return HashCode.Combine(Normal, D);
        }
        
        //What is it????
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