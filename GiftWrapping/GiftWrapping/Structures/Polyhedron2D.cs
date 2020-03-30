using System.Collections.Generic;
using System.ComponentModel.Design;

namespace GiftWrapping.Structures
{
    public class Polyhedron2D : IHyperface
    {
        public int Dim { get;  set; }
        public IList<Point> Faces { get; set; }
        public int NumberEdges { get; }
        public IList<IHyperface> GetNeighboringFaces()
        {
            throw new System.NotImplementedException();
        }
    }
}