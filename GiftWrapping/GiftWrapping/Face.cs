using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class Face
    {
        private IFace _convexHull;

        public  Hyperplane Hyperplane;
        public List<Face> AdjFaces { get; }
        public IFace ConvexHull
        {
            get => _convexHull;
            set
            {
                _convexHull = value;
                _convexHull.Hyperplane = Hyperplane;
            }
        }

        public Face(Hyperplane hyperplane)
        {
            this.Hyperplane = hyperplane;
            AdjFaces = new List<Face>();
        }
    }
}