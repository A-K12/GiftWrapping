using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace GiftWrapping.Structures
{
    public class ConvexFace2D : IConvexFace
    {
        public IList<Point> Points { get; }
        public IList<IConvexFace> AdjacentFaces { get; set; }
        public IList<IConvexFace> InnerFaces { get; set; }
        public Hyperplane Hyperplane { get; set; }
        public int Dimension { get;  }
        public ConvexFace2D()
        {
            Points = new List<Point>();
            AdjacentFaces = new List<IConvexFace>();
        }
    }
}