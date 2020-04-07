using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class Hyperface:IHyperface
    {
        public int Dim { get; }

        private ICollection<IHyperface> hyperfaces;

        private IDictionary<int, int[]> NeighboringFaces { get;  }

        public Hyperface()
        {
            hyperfaces = new List<IHyperface>();
        }

        public void AddHyperface(Hyperface hyperface)
        {
            hyperfaces.Add(hyperface);
        }
    }
}