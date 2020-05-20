using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GiftWrapping.Structures
{
    public class ConvexHull2d:IPointFace
    {
        public int Dimension { get; }
        public List<ICell> AdjacentCells { get; private set; }
        public IList<PlanePoint> Points { get; }

        public Hyperplane Hyperplane { get; set; }

        public IEnumerable<PlanePoint> GetPoints()
        {
            return Points;
        }

        public void AddPoint(PlanePoint point)
        {
            Points.Add(point);
        }

        public ConvexHull2d(Hyperplane hyperplane = default)
        {
            Dimension = 2;
            Hyperplane = hyperplane;
            Points = new List<PlanePoint>();
            AdjacentCells = new List<ICell>();
        }

        public ConvexHull2d(IEnumerable<PlanePoint> points)
        {
            Dimension = 2;
            Points = new List<PlanePoint>(points);
            AdjacentCells = new List<ICell>();
        }

        public bool Equals(IPointFace other)
        {
            IEnumerable<Point> points = other.GetPoints();
            return Dimension == other.Dimension && Points.All(points.Contains);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IPointFace)obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            for (int i = 0; i < Points.Count; i++)
                res += Points[i].GetHashCode();
            res += Dimension.GetHashCode();
            return res;
        }
    }
}