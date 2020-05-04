using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class ConvexFace : IConvexFace
    {
        public Hyperplane Hyperplane { get; set; }
        public int Dimension { get; }
        public IList<IConvexFace> InnerFaces { get; set; }
        public IList<IConvexFace> AdjacentFaces { get; set; }
        public ConvexFace(int dimension)
        {
            Dimension = dimension;
            InnerFaces = new List<IConvexFace>();
            AdjacentFaces = new List<IConvexFace>();
        }
    }
}