using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class ConvexHull : IFace
    {
        private readonly List<ICell> _adjacentCells, _innerCells;
        public int Dimension { get; }
        public IEnumerable<ICell> AdjacentCells => _adjacentCells;
        public IEnumerable<ICell> InnerCells => _innerCells;
        public void AddAdjacentCell(ICell cell) => _adjacentCells.Add(cell);

        public void AddInnerCell(ICell cell) => _innerCells.Add(cell);
        public Hyperplane Hyperplane { get; set; }
 
        public ConvexHull(int dimension)
        {
            Dimension = dimension;
            _innerCells = new List<ICell>();
            _adjacentCells = new List<ICell>();
        }

        public ConvexHull(Hyperplane hyperplane)
        {
            Hyperplane = hyperplane;
            Dimension = hyperplane.Dimension;
            _innerCells = new List<ICell>();
            _adjacentCells = new List<ICell>();
        }
        public ICollection<PlanePoint> GetPoints()
        {
            HashSet<PlanePoint> points = new HashSet<PlanePoint>();
            foreach (ICell innerFace in InnerCells)
            {
                points.UnionWith(innerFace.GetPoints());
            }

            return points;
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IFace) obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (ICell cell in _innerCells)
                res += cell.GetHashCode();

            res += Dimension.GetHashCode();
            return res;
        }

        public bool Equals(ICell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            ConvexHull convexHull = (ConvexHull)other;
            return Dimension == other.Dimension &&
                   _innerCells.Count == convexHull._innerCells.Count && 
                   _innerCells.All(convexHull._innerCells.Contains);
        }
    }
}