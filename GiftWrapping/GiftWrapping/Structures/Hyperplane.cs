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
            D = hyperplane.D;
        }
        public Hyperplane(Point point, Vector normal)
        {
            Dim = normal.Dim;
            Normal = normal;
            _points = new List<Point>{point};
            D = ComputeD();
            Normalize();
        }

        private double ComputeD()
        {
            double d = 0;

            for (int i = 0; i < Normal.Dim; i++)
            {
                d -= Normal[i] * MainPoint[i];
            }

            return d;
        }

        private void Normalize()
        {
            D /= Normal.Length;
            Normal = Normal.Normalize();
        }

        public static Hyperplane CreatePlane(IList<Point> points)
        {
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Points don't have same dimension");
            }
            if (points.Count != points[0].Dim)
            {
                throw new ArgumentException("Number of points is not equal to dimension.");
            }
            Vector[] vectors = points.ToVectors();
            Matrix matrix = vectors.ToMatrix(); 

            Hyperplane hyperplane = CreatePlane(points.Last(), matrix);
            hyperplane._points.AddRange(points.SkipLast(1));

            return hyperplane;
        }
        public static Hyperplane CreatePlane(Point point, IList<Vector> vectors)
        {
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Vectors don't have same dimension");
            }
            if (point.Dim != vectors[0].Dim)
            {
                throw new ArgumentException("Vectors and points have different dimensions..");
            }

            return  CreatePlane(point, vectors.ToMatrix());
        }
        public static Hyperplane CreatePlane(Point point, Matrix matrix)
        {
            if (point.Dim - 1 > matrix.Rows)
            {
                throw new ArgumentException("The plane cannot be found . There are not enough vectors.");
            }

            Vector normal = ComputeNormal(matrix);
            Hyperplane hyperplane = new Hyperplane(point, normal);

            return hyperplane;
        }

        private static Vector ComputeNormal(Matrix leftSide)
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