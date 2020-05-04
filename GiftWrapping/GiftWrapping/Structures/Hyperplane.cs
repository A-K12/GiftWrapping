using System;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane : IEquatable<Hyperplane>
    {
        public IndexMap Mask { get; set; }
        private double D;
        public Point MainPoint { get; }
        public Vector Normal { get; private set; }

        public Point[] Points { get; set; }

        public int Dim { get; }

        public Hyperplane(Hyperplane h) : this(h.MainPoint, h.Normal, h.Mask)
        {
            Points = h.Points;
        }

        public Hyperplane(Point point, Vector normal, IndexMap mask)
        {
            if (mask.Length != normal.Dim)
                throw new ArgumentException("Mask and normal have different dimensions.");

            Dim = normal.Dim;
            Normal = normal.Normalize();
            MainPoint = point;
            Mask = mask;
            Points = new Point[Dim];
            ComputeD();
        }

        public Hyperplane(Point point, Vector normal)
        {
            if (point.Dim != normal.Dim)
                throw new ArgumentException("Point and normal have different dimensions.");

            Dim = normal.Dim;
            Normal = normal.Normalize();
            MainPoint = point;
            Points = new Point[Dim];
            Mask = new IndexMap(Dim);
            ComputeD();
        }

        private void ComputeD()
        {
            for (int i = 0; i < Mask.Length; i++) D += MainPoint[Mask[i]] * Normal[i];
            D /= Normal.Length;
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
            if (Dim != other.Dim) return false;
            List<double> quotients = new List<double>();
            for (int i = 0; i < other.Dim; i++)
            {
                if (Tools.EQ(Normal[i]) && Tools.EQ(other.Normal[i])) continue;
                quotients.Add(other.Normal[i] / Normal[i]);
            }

            if (Tools.NE(D) && Tools.NE(other.D)) quotients.Add(other.D / D);

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
            return HashCode.Combine(Normal, D);
        }

        public bool IsPointInPlane(Point point)
        {
            Vector v = Point.ToVector(MainPoint, point);
            return Tools.EQ(Normal * v);
        }
    }
}