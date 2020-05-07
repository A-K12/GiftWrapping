using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class ConvexHull2d:IFace2d
    {
        private Hyperplane _hyperplane;
        public int Dimension { get;}
        public List<ICell> AdjacentCells { get; set; }
        public List<Point> Points { get; set; }
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


        public ConvexHull2d(Hyperplane hyperplane)
        {
            Hyperplane = hyperplane ?? throw new ArgumentNullException(nameof(hyperplane));
            Dimension = _hyperplane.Dimension;
            AdjacentCells =new List<ICell>();
            Points = new List<Point>();
        }

        public ConvexHull2d()
        {
            _hyperplane= default;
            Dimension = 2;
            AdjacentCells = new List<ICell>();
            Points = new List<Point>();
        }

        public bool Equals(IFace2d other)
        {
            return Dimension == other.Dimension && Points.All(other.Points.Contains);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IFace2d) obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            for (int i = 0; i < Points.Count; i++)
                res += Points[i].GetHashCode();
            res += Dimension.GetHashCode();
            return res;
        }
    }
}