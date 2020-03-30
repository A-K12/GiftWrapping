using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public class Polihedron:IHyperface
    {
        private IList<IHyperface> hyperfaces;
        public int Dim { get; }
        public int NumberEdges { get; }
        public IList<IHyperface> GetNeighboringFaces()
        {
            throw new System.NotImplementedException();
        }
    }
}