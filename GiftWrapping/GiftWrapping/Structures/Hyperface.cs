using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class Hyperface : IHypeface
    {
        public int Dim { get; }

        private ICollection<IHypeface> Hyperfaces { get; }

        public IHypeface NeighboringFace { get; set; }

        public IHypeface ParentFace { get; set; }

        public Hyperface()
        {
            Hyperfaces = new List<IHypeface>();
        }

        public void AddHyperface(IHypeface hyperface)
        {
            Hyperfaces.Add(hyperface);
        }


        public bool Equals(Hyperface other)
        {
            return other.Dim.Equals(Dim) && other.Hyperfaces.Equals(Hyperfaces) &&
                   other.NeighboringFace.Equals(NeighboringFace)&& ParentFace.Equals(other.ParentFace);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Hyperface)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hyperfaces, Dim, NeighboringFace, ParentFace);
        }
    }
}