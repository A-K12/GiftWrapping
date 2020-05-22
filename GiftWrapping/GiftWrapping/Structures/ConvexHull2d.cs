using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using GiftWrapping.Helpers;

namespace GiftWrapping.Structures
{
    public class ConvexHull2d:IFace 
    {
        private readonly List<PlanePoint> _points;
        private readonly List<ICell> _adjacentCells;
        public int Dimension => 2;
        public IEnumerable<ICell> AdjacentCells => _adjacentCells;
        public IEnumerable<ICell> InnerCells => GetInnerCells();
        public Hyperplane Hyperplane { get; set; }

        public ConvexHull2d(Hyperplane hyperplane = default)
        {
            Hyperplane = hyperplane;
            _points = new List<PlanePoint>();
            _adjacentCells = new List<ICell>();
        }

        public ConvexHull2d(IEnumerable<PlanePoint> points)
        {
            _points = new List<PlanePoint>(points);
            _adjacentCells = new List<ICell>();
        }
        private IEnumerable<ICell> GetInnerCells()
        {
            Edge firstEdge = new Edge(_points[^1], _points[0]);
            firstEdge.Hyperplane = HyperplaneHelper.Create(firstEdge.GetPoints().ToArray());

            yield return firstEdge;
            
            for (int i = 0; i < _points.Count - 1; i++)
            {
                Edge nextEdge = new Edge(_points[i], _points[i + 1]);
                nextEdge.Hyperplane = HyperplaneHelper.Create(nextEdge.GetPoints().ToArray());
                yield return nextEdge;
            }
        }

        public void AddAdjacentCell(ICell cell)
        {
            if (cell.Dimension == Dimension)
            {
                _adjacentCells.Add(cell);
            }
        }

        public ICollection<PlanePoint> GetPoints()
        {
            return _points;
        }

        public void GoToOriginalPoints()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i] = _points[i].OriginalPoint;
            }
        }

        public void AddPoint(PlanePoint point)
        {
            _points.Add(point);
        }

        public bool Equals(ConvexHull2d other)
        {
            IEnumerable<Point> points = other.GetPoints();
            return Dimension == other.Dimension && _points.All(points.Contains);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConvexHull2d)obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (PlanePoint point in _points)
                res += point.GetHashCode();
            res += Dimension.GetHashCode();
            return res;
        }
    }
}