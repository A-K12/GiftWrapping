using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GiftWrapping.Structures
{
    public class ConvexFace : IConvexFace
    {
        public Vector Normal { get; set; }
        public int Dimension { get; }
        private IList<IConvexFace> InnerFaces { get; }
        public IList<IConvexFace> AdjacentFaces { get; set; }
        public ConvexFace()
        {
            InnerFaces = new List<IConvexFace>();
            AdjacentFaces = new List<IConvexFace>();
        }
    }
}