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
        private readonly List<ICell> _adjacentCells, _innerCells;
        public int Dimension => 2;
        public IEnumerable<ICell> AdjacentCells => _adjacentCells;
        public IEnumerable<ICell> InnerCells => _innerCells;
        public Hyperplane Hyperplane { get; set; }

        public ConvexHull2d(IEnumerable<PlanePoint> points)
        {
            _points = new List<PlanePoint>(points);
            _adjacentCells = new List<ICell>();
            _innerCells = GetInnerCells().ToList();
        }
        private IEnumerable<ICell> GetInnerCells()
        {
            Edge firstEdge = new Edge(_points[^1], _points[0]);
            firstEdge.Hyperplane = HyperplaneHelper.Create(firstEdge.GetPoints().ToArray());
            firstEdge.Hyperplane.SetOrientationNormal(_points);
            yield return firstEdge;
            
            for (int i = 0; i < _points.Count - 1; i++)
            {
                Edge nextEdge = new Edge(_points[i], _points[i + 1]);
                nextEdge.Hyperplane = HyperplaneHelper.Create(nextEdge.GetPoints().ToArray());
                nextEdge.Hyperplane.SetOrientationNormal(_points);
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


        public bool Equals(ICell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            ConvexHull2d convexHull = (ConvexHull2d) other;
            return Dimension == other.Dimension && 
                   _points.Count == convexHull._points.Count && 
                   _points.All(convexHull._points.Contains);
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