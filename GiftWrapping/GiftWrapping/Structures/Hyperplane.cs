using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane
    {
        public double NumericVariable { get; private set; }
        public Vector Normal { get; private set; }
        public PlanePoint MainPoint { get; set; }
        public int Dimension { get; }
        public Vector[] Basis { get; set; }

        public Hyperplane(Hyperplane h) : this(h.MainPoint, h.Normal)
        {
            Basis = h.Basis;
        }

        public Hyperplane(PlanePoint point, Vector normal)
        {
            if (point.Dim != normal.Dim)
                throw new ArgumentException("Point and normal have different dimensions.");

            Dimension = normal.Dim;
            Normal = normal.Normalize();
            MainPoint = point;
            ComputeNumVariable();
        }

        private void ComputeNumVariable()
        {
            NumericVariable = MainPoint * Normal;
        }

        public double Angle(Hyperplane hyperplane)
        {
            return Vector.Angle(Normal, hyperplane.Normal);
        }

        public double Cos(Hyperplane hyperplane)
        {
            return Normal *hyperplane.Normal;//Normal.Length = 1
        }

        public int Side(Point point)
        {
            Vector v = Point.ToVector(MainPoint, point);

            return Tools.CMP(Normal * v);
        }

        public void ReorientNormal()
        {
            Normal = -Normal;
            ComputeNumVariable();
        }

        public bool Equals(Hyperplane other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Dimension != other.Dimension) return false;
            List<double> quotients = new List<double>();
            for (int i = 0; i < other.Dimension; i++)
            {
                if (Tools.NE(Normal[i]))
                {
                    quotients.Add(other.Normal[i] / Normal[i]);
                }
                else
                {
                    if (Tools.NE(other.Normal[i]))
                    {
                        return false;
                    }
                }
            }

            if (Tools.NE(NumericVariable) && Tools.NE(other.NumericVariable))
                quotients.Add(other.NumericVariable / NumericVariable);

            return quotients.Count == 0 || quotients.All(d => Tools.EQ(d, quotients[0]));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Hyperplane) obj);
        }

        public override int GetHashCode()
        {

            return HashCode.Combine(Normal, NumericVariable);
        }

        public bool IsPointInPlane(Point point)
        {
            Vector v = Point.ToVector(MainPoint, point);
            bool result = Tools.EQ(Normal * v);
            return result;
        }

        public PlanePoint ConvertPoint(PlanePoint point)
        {
            if (point.Dim != MainPoint.Dim)
            {
                throw new ArgumentException("The planePoint has the wrong dimension.");
            }
            Point p1 = point - MainPoint;
            double[] newPoint = Basis.Select((vector => vector * p1)).ToArray();

            return new PlanePoint(newPoint, point);
        }

        public Vector ConvertVector(Vector vector)
        {
            if (vector.Dim >= MainPoint.Dim)
            {
                throw new ArgumentException("The planePoint has the wrong dimension.");
            }
            double[] newVector = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < vector.Dim; j++)
                {
                    newVector[i] += vector[j] * Basis[j][i];
                }
            }
            return new Vector(newVector);
        }

        public void OrthonormalBasis()
        {
            Vector[] b = Basis.GetOrthonormalBasis();
            Basis = b;
        }
        public void SetOrientationNormal(IEnumerable<PlanePoint> innerPoints)
        {
            foreach (PlanePoint planePoint in innerPoints)
            {
                Vector vector = Point.ToVector(MainPoint, planePoint.GetPoint(Dimension));
                double dotProduct = vector * Normal;
                if (Tools.EQ(dotProduct)) continue;
                if (Tools.GT(dotProduct))
                {
                    ReorientNormal();
                }
                return;
            }
            throw new ArgumentException("All points lie on the plane.");
        }

        public static Hyperplane Create(IList<PlanePoint> points)
        {
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Basis don't have same dimension");
            }
            if (points.Count != points[0].Dim)
            {
                throw new ArgumentException("Number of points is not equal to dimension.");
            }
            Vector[] vectors = points.ToVectors();
            Hyperplane hyperplane = Create(points.First(), vectors);


            return hyperplane;
        }
        public static Hyperplane Create(PlanePoint point, Vector[] vectors)
        {
            if (!vectors.HaveSameDimension())
            {
                throw new ArgumentException("Vectors don't have same dimension");
            }
            if (point.Dim != vectors[0].Dim)
            {
                throw new ArgumentException("Vectors and points have different dimensions.");
            }

            Matrix leftSide = vectors.ToHorizontalMatrix();

            Vector normal = ComputeNormal(leftSide);

            return new Hyperplane(point, normal)
            {
                Basis = vectors.GetOrthonormalBasis()
            };
        }


        private static Vector ComputeNormal(Matrix leftSide)
        {
            Vector rightSide = new Vector(leftSide.Rows);
            try
            {
                return GaussWithChoiceSolveSystem.FindAnswer(leftSide, rightSide);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Vectors are linearly dependent");
            }
        }
    }
}