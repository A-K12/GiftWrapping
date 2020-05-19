namespace GiftWrapping.Structures
{
    public class PlanePoint:Point
    {
        private PlanePoint _originalPoint;
        public PlanePoint OriginalPoint
        {
            get => _originalPoint ?? (this);
            set => _originalPoint=value;
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
            OriginalPoint = p.OriginalPoint;
        }
        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != this.GetType()) return false;
        //    return Equals((PlanePoint)obj);
        //}
        //public bool Equals(PlanePoint other)
        //{
        //    if (ReferenceEquals(null, other)) return false;
        //    if (ReferenceEquals(this, other)) return true;
        //    return OriginalPoint.Equals(other);
        //}

        //public override int GetHashCode()
        //{
        //    return OriginalPoint.GetHashCode();
        //}
    }
}