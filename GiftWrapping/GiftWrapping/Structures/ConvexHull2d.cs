using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using GiftWrapping.Helpers;

namespace GiftWrapping.Structures
{
    public class ConvexHull2d:IConvexHull 
    {
        public int Dimension { get; }
        public List<ICell> Faces { get; }
        private readonly List<PlanePoint> _points;

        public ConvexHull2d(IEnumerable<PlanePoint> points)
        {
            Dimension = 2;
            _points = new List<PlanePoint>(points);
            Faces = new List<ICell>();
            ComputeData();
        }
        private void ComputeData()
        {
            Edge edge = new Edge(_points[^1], _points[0]);
            edge.Hyperplane = HyperplaneBuilder.Create(edge.GetPoints().ToArray());
            edge.Hyperplane.SetOrientationNormal(_points);
            AddInnerCell(edge);
            for (int i = 0; i < _points.Count - 1; i++)
            {
                edge = new Edge(_points[i], _points[i + 1]);
                edge.Hyperplane = HyperplaneBuilder.Create(edge.GetPoints().ToList());
                edge.Hyperplane.SetOrientationNormal(_points);
                AddInnerCell(edge);
            }
        }
        
        private void AddInnerCell(ICell cell)
        {
            Faces.Add(cell);
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