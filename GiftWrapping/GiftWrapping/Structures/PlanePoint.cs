using Microsoft.Win32.SafeHandles;

namespace GiftWrapping.Structures
{
    public class PlanePoint:Point
    {
        public Point OriginPoint { get; set; }
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
    }
}