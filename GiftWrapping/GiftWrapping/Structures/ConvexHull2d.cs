using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GiftWrapping.Structures
{
    public class ConvexHull2d:IPointFace
    {
        public int Dimension { get; }
        public List<ICell> AdjacentCells { get; private set; }

        private Hyperplane _hyperplane;

        private readonly List<PlanePoint> _points;
        public PlanePoint this[int i] => _points[i];
        public Hyperplane Hyperplane
        {
            set
            {
                if (value.Dimension != 2)
                {
                    throw new ArgumentException("Hyperplane is not two-dimensional.");
                }

                _hyperplane = value;
            }
            get => _hyperplane;
        }

        public IEnumerable<PlanePoint> GetPoints()
        {
            return _points;
        }

        public void AddPoint(PlanePoint point)
        {
            _points.Add(point);
        }

        public ConvexHull2d(Hyperplane hyperplane = default)
        {
            Dimension = 2;
            _hyperplane = hyperplane;
            _points = new List<PlanePoint>();
            AdjacentCells = new List<ICell>();
        }

        public ConvexHull2d(IEnumerable<PlanePoint> points)
        {
            Dimension = 2;
            _points = new List<PlanePoint>(points);
            AdjacentCells = new List<ICell>();
        }

        public bool Equals(IPointFace other)
        {
            IEnumerable<PlanePoint> points = other.GetPoints();
            return Dimension == other.Dimension && _points.All(points.Contains);
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
            for (int i = 0; i < _points.Count; i++)
                res += _points[i].GetHashCode();
            res += Dimension.GetHashCode();
            return res;
        }

    }
}