using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class Edge2d
    {
        
        public Point[] Points { get;  set; }

        public Edge2d(Point p1, Point p2)
        {
            if(p1 == p2) throw new ArgumentException("Points are equal.");
            Points = new[] {p1, p2};
        }


        public Edge2d(Point[] points)
        {
            Points = points;
        }

        public bool Equals(Edge2d other)
        {
            return Points.All(other.Points.Contains);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge2d) obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            for (int i = 0; i < Points.Length; i++)
                res += Points[i].GetHashCode();
            return res;
        }
    }
}