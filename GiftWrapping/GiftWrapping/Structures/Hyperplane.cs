using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.LinearEquations;

namespace GiftWrapping.Structures
{
    public class Hyperplane: IEquatable<Hyperplane>
    {
        public Point MainPoint { get; }

        public Vector Normal { get; private set; }

        public int Dim { get; }
        
        private double D;

        private bool[] _map;

        public bool[] ProjectionMap
        {
            get => _map;
            set
            {
                if (value.Length == Dim)
                    _map = value;
            }
        }

        public Hyperplane(Hyperplane hyperplane)
        {
            Dim = hyperplane.Dim;
            Normal = hyperplane.Normal;
            MainPoint = hyperplane.MainPoint;
            _map = hyperplane._map;
            ComputeD();
        }

        public Hyperplane(Point point, Vector normal)
        {
            Dim = normal.Dim;
            Normal = normal;
            MainPoint = point;
            ComputeD();
            Normal.Normalize();
        }

        private void ComputeD()
        {
            D = MainPoint * Normal / Normal.Length;
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

        public bool IsPointInPlane(Point point)
        {
            Vector v = Point.ToVector(MainPoint, point);
            return Tools.EQ(Normal*v);
        }

    }   
}