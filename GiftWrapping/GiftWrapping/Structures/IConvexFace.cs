using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IConvexFace
    {
        IList<IConvexFace> AdjacentFaces { get; set; }
        IList<IConvexFace> InnerFaces { get; set; }
        Hyperplane Hyperplane { get; set; }
        int Dimension { get; }
    }
}