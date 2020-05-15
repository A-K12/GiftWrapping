using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane
    {
        public Vector[] _basis;
        public Vector _offSet;
        private double NumericVariable { get; set; }
        public Vector Normal { get; private set; }
        public Point MainPoint { get; set; }
        public int Dimension { get; }

        public Vector[] Basis
        {
            get => _basis;
            set
            {
                _basis = value;
                _offSet = ComputeOffset();
            }
        }

        private Vector ComputeOffset()
        {
            Matrix basis = _basis.ToHorizontalMatrix();
            return basis * MainPoint;
        }

        public Hyperplane(Hyperplane h) : this(h.MainPoint, h.Normal)
        {
            Basis = h.Basis;
        }

        public Hyperplane(Point point, Vector normal)
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

        public int Side(Point point)
        {
            Vector v = Point.ToVector(MainPoint, point);

            return Tools.CMP(Normal * v);
        }

        public void ReorientNormal()
        {
            Normal = -Normal;
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
            return Tools.EQ(Normal * v);
        }

        public PlanePoint ConvertPoint(Point point)
        {
            Point originalPoint = (point is PlanePoint planePoint) ? planePoint.OriginPoint : point;
            Point p1 = point - MainPoint;
            double[] newPoint = Basis.Select((vector => vector * p1)).ToArray();
     
            return new PlanePoint(newPoint, originalPoint);
        }
    }
}