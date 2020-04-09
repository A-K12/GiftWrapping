using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public class ConvexHull2d:IConvexHull
    {
        private ICollection<Point> points;
        public int Dim { get; }

        public ConvexHull2d()
        {
            points= new List<Point>();
        }

        protected bool Equals(ConvexHull2d other)
        {
            return other.Dim.Equals(Dim) && other.points.Equals(points);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConvexHull2d) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dim, points);
        }
    }
}