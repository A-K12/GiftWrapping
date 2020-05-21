using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class Edge:IEdge
    {
        private readonly PlanePoint[] _points;
        public int Dimension => 2;
        public Hyperplane Hyperplane { get; set; }
        public ICollection<PlanePoint> GetPoints()
        {
            
            return _points;
        }
        public Edge(PlanePoint p1, PlanePoint p2)
        {
            if (p1.Dim != p2.Dim) throw new ArgumentException("_points have different dimensions.");
            if (p1 == p2) throw new ArgumentException("Objects are equal.");
            _points = new[] {p1, p2};
        }

        public Edge()
        {
            _points = new PlanePoint[2];
        }
        public override int GetHashCode()
        {
            int res = 0;
            foreach (PlanePoint point in _points)
                res += point.GetHashCode();

            return res;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge)obj);
        }

        public bool Equals(Edge obj)
        {
            return obj != null && _points.All(obj._points.Contains);
        }

    }
}