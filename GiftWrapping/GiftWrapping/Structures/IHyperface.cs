using System;
using System.Collections.Generic;
using System.Text;

namespace GiftWrapping.Structures
{
    public interface IHyperface:IPolyhedron
    {
        public int NumberEdges { get; }
        IList<IHyperface> GetNeighboringFaces();
    }
}
