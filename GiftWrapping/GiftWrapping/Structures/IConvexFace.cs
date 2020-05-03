using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IConvexFace
    {
        public IList<IConvexFace> AdjacentFaces { get; set; }
        public Hyperplane Hyperplane { get; set; }
        int Dimension { get; }
    }
}