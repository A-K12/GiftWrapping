using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace GiftWrapping.Structures
{
    public class Edge : IHypeface
    {
        public Point[] Points { get; }
        public IHypeface NeighboringFace { get; set; }
        public IHypeface ParentFace { get; set; }
        public int Dim { get; }

        public Edge(Point first, Point second)
        {
            Points = new Point[2] {first, second};
        }

        public bool Equals(Edge other)
        {
            return other.Dim.Equals(Dim) && other.Points.Equals(Points);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dim, Points);
        }
    }
}