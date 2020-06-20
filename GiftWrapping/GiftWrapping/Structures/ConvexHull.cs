using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class ConvexHull : IConvexHull
    {
        public int Dimension { get; }
        public List<ICell> Faces { get; }

        public ICollection<PlanePoint> GetPoints()
        {
            HashSet<PlanePoint> points = new HashSet<PlanePoint>();
            foreach (ICell innerFace in Faces)
            {
                points.UnionWith(innerFace.GetPoints());
            }

            return points;
        }

        public void AddInnerCell(ICell cell)
        {
            Faces.Add(cell);
        }
        public ConvexHull(int dimension)
        {
            Dimension = dimension;
            Faces = new List<ICell>();
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
            foreach (ICell cell in Faces)
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
                   Faces.Count == convexHull.Faces.Count &&
            GetPoints().All(other.GetPoints().Contains);
        }
    }
}