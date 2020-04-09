using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class ConvexHull:IConvexHull
    {
       

        public int Dim { get; }

        private ICollection<IConvexHull> _hyperfaces;

        private IDictionary<int, int[]> _neighboringFaces;
        public int[] NeighboringFaces(int i) => _neighboringFaces[i];

        public ConvexHull()
        {
            _hyperfaces = new List<IConvexHull>();
            _neighboringFaces = new Dictionary<int, int[]>();
        }

        public void AddHyperface(ConvexHull hyperface)
        {
            _hyperfaces.Add(hyperface);
        }


        protected bool Equals(ConvexHull other)
        {
            return other.Dim.Equals(Dim) && other._hyperfaces.Equals(_hyperfaces) &&
                   other._neighboringFaces.Equals(_neighboringFaces);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConvexHull)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_hyperfaces, Dim,_neighboringFaces);
        }
    }
}