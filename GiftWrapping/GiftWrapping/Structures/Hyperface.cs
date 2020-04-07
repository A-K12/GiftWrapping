using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class Hyperface:IHyperface
    {
        public int Dim { get; }

        private ICollection<IHyperface> _hyperfaces;

        private IDictionary<int, int[]> _neighboringFaces;
        public int[] NeighboringFaces(int i) => _neighboringFaces[i];

        public Hyperface()
        {
            _hyperfaces = new List<IHyperface>();
            _neighboringFaces = new Dictionary<int, int[]>();
        }

        public void AddHyperface(Hyperface hyperface)
        {
            _hyperfaces.Add(hyperface);
        }

    }
}