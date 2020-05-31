using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public class PlanePoint:Point
    {

        private PlanePoint _originalPoint;
        public PlanePoint OriginalPoint
        {
            get => _originalPoint ?? (this);
        }
        public PlanePoint GetPoint(int dimension)
        {
            if (dimension == Dim) return this;
            return _originalPoint.GetPoint(dimension) ??
                   throw new ArgumentException("There is no point of this dimension.");
        }
        public PlanePoint(int n, PlanePoint originalPoint) : base(n)
        {
            _originalPoint = originalPoint;
        }
        public PlanePoint(double[] np, PlanePoint originalPoint) : base(np)
        {
            _originalPoint = originalPoint;
        }

        public PlanePoint(Point p, PlanePoint originalPoint) : base(p)
        {
            _originalPoint = originalPoint;
        }

        public PlanePoint(int n) : base(n)
        {
        }
        public PlanePoint(double[] np) : base(np)
        {
        }

        public PlanePoint(Point p) : base(p)
        {
        }

        public PlanePoint(PlanePoint p) : base(p)
        {
            _originalPoint = p.OriginalPoint;
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
            return base.Equals((Point)other);
        }

        public override int GetHashCode()
        {
            if( ReferenceEquals(null, _originalPoint))
            {
                return base.GetHashCode();
            }
            return  _originalPoint.GetHashCode();
        }
    }
}