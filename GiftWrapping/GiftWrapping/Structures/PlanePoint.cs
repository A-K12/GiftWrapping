using System;
using Microsoft.Win32.SafeHandles;

namespace GiftWrapping.Structures
{
    public class PlanePoint:Point
    {
        public Point OriginPoint { get; }
        public PlanePoint(int n, Point originPoint) : base(n)
        {
            OriginPoint = originPoint;
        }

        public PlanePoint(double[] np, Point originPoint) : base(np)
        {
            OriginPoint = originPoint;
        }

        public PlanePoint(Point p, Point originPoint) : base(p)
        {
            OriginPoint = originPoint;
        }

        public PlanePoint(PlanePoint p) : base(p)
        {
            OriginPoint = p.OriginPoint;
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
            return OriginPoint.CompareTo(other.OriginPoint) == 0;
        }

        public override int GetHashCode()
        {
           return OriginPoint.GetHashCode();
        }

    }
}