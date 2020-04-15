using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class ConvexHull : IConvexHull
    {
        public int Dim { get; }

        private ICollection<IConvexHull> _hyperfaces;

        public IConvexHull NeighboringFace { get; set; }

        public IConvexHull ParentFace { get; set; }

        public ConvexHull()
        {
            _hyperfaces = new List<IConvexHull>();
        }

        public void AddHyperface(IConvexHull hyperface)
        {
            _hyperfaces.Add(hyperface);
        }


        protected bool Equals(ConvexHull other)
        {
            return other.Dim.Equals(Dim) && other._hyperfaces.Equals(_hyperfaces) &&
                   other.NeighboringFace.Equals(NeighboringFace)&& ParentFace.Equals(other.ParentFace);
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
            return HashCode.Combine(_hyperfaces, Dim, NeighboringFace, ParentFace);
        }
    }
}