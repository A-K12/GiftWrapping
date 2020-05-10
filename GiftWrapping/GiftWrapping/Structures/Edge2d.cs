using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class Edge2d:IPointFace
    {
        private Hyperplane _hyperplane;
        public int Dimension => 2;
        public List<ICell> AdjacentCells { get; private set; }
        public Point[] Points { get; private set; }

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

        public IEnumerable<Point> GetPoints() 
        {
            return Points;
        }


        public Edge2d(Hyperplane hyperplane)
        {
            Hyperplane = hyperplane ?? throw new ArgumentNullException(nameof(hyperplane));
            Init();
        }

        public Edge2d()
        {
            _hyperplane= default;
            Init();
        }

        private void Init()
        {
            AdjacentCells = new List<ICell>();
            Points = new Point[Dimension];
        }

        public bool Equals(IPointFace other)
        {
            return Dimension == other.Dimension && Points.All(other.Points.Contains);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IPointFace) obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            for (int i = 0; i < Points.Length; i++)
                res += Points[i].GetHashCode();
            res += Dimension.GetHashCode();
            return res;
        }
    }
}