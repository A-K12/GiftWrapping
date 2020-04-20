using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public class ConvexFaceInternal
    {
        public Vector Normal { get; set; }
        public int Dimension { get; set; }
        public IList<int> InnerFaces { get; set; }
        public IList<int> FaceElements { get; set; }
        public int FaceIndex { get; set; }
        public ConvexFaceInternal()
        {
            InnerFaces = new List<int>();
            FaceElements = new List<int>();
        }
    }
}