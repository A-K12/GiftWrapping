using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane
    {
        public IndexMap Mask { get; set; }
        private double NumericVariable { get; set; }
        public Vector Normal { get; private set; }
        public Vector[] Basis { get; set; }
        public Point MainPoint { get; set; }
        public int Dimension { get; }

        public Hyperplane(Hyperplane h) : this(h.MainPoint, h.Normal, h.Mask)
        {
            Basis = h.Basis;
        }

        public Hyperplane(Point point, Vector normal, IndexMap mask)
        {
            if (mask.Length != normal.Dim)
                throw new ArgumentException("Mask and normal have different dimensions.");

            Dimension = normal.Dim;
            Normal = normal.Normalize();
            Mask = mask;
            MainPoint = point;
            Basis = new Vector[Dimension];
            Basis[0] = point;
            ComputeNumVariable();
        }

        public Hyperplane(Point point, Vector normal)
        {
            if (point.Dim != normal.Dim)
                throw new ArgumentException("Point and normal have different dimensions.");

            Dimension = normal.Dim;
            Normal = normal.Normalize();
            Basis = new Vector[Dimension];
            Basis[0] = point;
            Mask = new IndexMap(Dimension);
            MainPoint = point;
            ComputeNumVariable();
        }

        private void ComputeNumVariable()
        {
            for (int i = 0; i < Mask.Length; i++) NumericVariable += MainPoint[Mask[i]] * Normal[i];
            NumericVariable /= Normal.Length;
        }

        public double Angle(Hyperplane hyperplane)
        {
            return Vector.Angle(Normal, hyperplane.Normal);
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
            if (Dimension != other.Dimension) return false;
            List<double> quotients = new List<double>();
            for (int i = 0; i < other.Dimension; i++)
            {
                if (Tools.EQ(Normal[i]) && Tools.EQ(other.Normal[i])) continue;
                quotients.Add(other.Normal[i] / Normal[i]);
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
    }
}