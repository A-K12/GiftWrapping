using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public class PlanePoint:Point
    {

        private readonly Point _originalPoint;

        private readonly PlanePoint _previousPoint;
      
        public PlanePoint GetPoint(int dimension)
        {
            if (dimension == Dim) return this;
            return _previousPoint.GetPoint(dimension) ??
                   throw new ArgumentException("There is no point of this dimension.");
        }

        public PlanePoint(int n, PlanePoint point) : base(n)
        {
            _originalPoint = point._originalPoint;
            _previousPoint = point;

        }
        public PlanePoint(double[] np, PlanePoint point) : base(np)
        {
            _originalPoint = point._originalPoint;
            _previousPoint = point;
        }

        public PlanePoint(Point p, PlanePoint point) : base(p)
        {
            _originalPoint = point._originalPoint;
            _previousPoint = point;
        }

        public PlanePoint(int n) : base(n)
        {
            _originalPoint = new Point(n);
        }
        public PlanePoint(double[] np) : base(np)
        {
            _originalPoint = new Point(np);
        }

        public PlanePoint(Point p) : base(p)
        {
            _originalPoint = new Point(p);
        }

        public PlanePoint(PlanePoint p) : base(p)
        {
            _originalPoint = p._originalPoint;
            _previousPoint = p._previousPoint;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlanePoint)obj);
        }
        public bool Equals(PlanePoint other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _originalPoint.Equals(other._originalPoint);
        }

        public override int GetHashCode()
        {
            return _originalPoint.GetHashCode();
        }
    }
}