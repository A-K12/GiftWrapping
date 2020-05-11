using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class ConvexHull : IFace
    {
        public ConvexHull(int dimension)
        {
            Dimension = dimension;
            InnerFaces = new List<ICell>();
            AdjacentCells = new List<ICell>();
        }

        public ConvexHull(Hyperplane hyperplane)
        {
            Hyperplane = hyperplane;
            Dimension = hyperplane.Dimension;
            InnerFaces = new List<ICell>();
            AdjacentCells = new List<ICell>();
        }

        protected bool Equals(IFace other)
        {
            return Dimension == other.Dimension && InnerFaces.All(other.InnerFaces.Contains);
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
            for (int i = 0; i < InnerFaces.Count; i++)
                res += InnerFaces[i].GetHashCode();
            res += Dimension.GetHashCode();
            res += InnerFaces.Count;
            return res;
        }

        public int Dimension { get; }
        public List<ICell> AdjacentCells { get; }
        public Hyperplane Hyperplane { get; set; }
        public IEnumerable<Point> GetPoints()
        {
            HashSet<Point> points = new HashSet<Point>();
            foreach (ICell innerFace in InnerFaces)
            {
                points.UnionWith(innerFace.GetPoints());
            }

            return points;
        }

        public List<ICell> InnerFaces { get; }
       
    }
}