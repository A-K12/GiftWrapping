﻿using System;
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
        public List<ICell> Cells { get; }
        private readonly List<PlanePoint> _points;

        public ConvexHull2d(IList<PlanePoint> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if(points.Count < 3) throw  new ArgumentException("There are not enough points to build a convex hull.");
            Dimension = 2;
            _points = new List<PlanePoint>(points);
            Cells = new List<ICell>();
            ComputeData();
        }
        private void ComputeData()
        {
            Edge edge = new Edge(_points[^1], _points[0]);
            edge.Hyperplane.SetOrientationNormal(_points);
            AddInnerCell(edge);
            for (int i = 0; i < _points.Count - 1; i++)
            {
                edge = new Edge(_points[i], _points[i + 1]);
                edge.Hyperplane.SetOrientationNormal(_points);
                AddInnerCell(edge);
            }
        }
        
        

        private void AddInnerCell(ICell cell)
        {
            Cells.Add(cell);
        }
        
        public ICollection<PlanePoint> GetPoints()
        {
            return _points;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConvexHull2d)obj);
        }

        public bool Equals(ConvexHull2d other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            
            return Dimension == other.Dimension &&
                   _points.Count == other._points.Count &&
                   _points.All(other._points.Contains);
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