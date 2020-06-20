using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping.Structures
{
    public class Face:IFace
    {
        public bool Equals(ICell other)
        {
            throw new System.NotImplementedException();
        }

        public int Dimension { get; }
        public Hyperplane Hyperplane { get; set; }
        public List<ICell> AdjacentCells { get; }
        public ICollection<PlanePoint> GetPoints()=> ConvexHull.GetPoints();
        public IConvexHull ConvexHull { get; set; }

        public Face(int dimension)
        {
            Dimension = dimension;
            AdjacentCells = new List<ICell>();
        }

        public Face(Hyperplane hyperplane)
        {
            Hyperplane = hyperplane;
            Dimension = hyperplane.Dimension;
            AdjacentCells = new List<ICell>();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Face)obj);
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (ICell cell in ConvexHull.Faces)
                res += cell.GetHashCode();

            res += Dimension.GetHashCode();
            return res;

        }

        public bool Equals(Face other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            
            return Dimension == other.Dimension &&
                   ConvexHull.Faces.Count == other.ConvexHull.Faces.Count &&
                   GetPoints().All(other.GetPoints().Contains);
        }

    }
}