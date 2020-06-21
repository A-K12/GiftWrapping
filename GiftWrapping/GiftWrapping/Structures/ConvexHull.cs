using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class ConvexHull : IConvexHull
    {
        public int Dimension { get; }
        public List<ICell> Cells { get; }

        public ICollection<PlanePoint> GetPoints()
        {
            HashSet<PlanePoint> points = new HashSet<PlanePoint>();
            foreach (ICell innerFace in Cells)
            {
                points.UnionWith(innerFace.GetPoints());
            }

            return points;
        }

        public void AddInnerCell(ICell cell)
        {
            Cells.Add(cell);
        }
        public ConvexHull(int dimension)
        {
            Dimension = dimension;
            Cells = new List<ICell>();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConvexHull) obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (ICell cell in Cells)
                res += cell.GetHashCode();
            
            res += Dimension.GetHashCode();
            return res;
      
        }

        public bool Equals(ConvexHull other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            ConvexHull convexHull = (ConvexHull)other;
            return Dimension == other.Dimension &&
                   Cells.Count == convexHull.Cells.Count &&
            Cells.All(other.Cells.Contains);
        }
    }
}