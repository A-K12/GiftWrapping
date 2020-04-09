using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class GiftWrapping
    {
        public IConvexHull ComputeConvexHull(IList<Point> points)
        {
            return new ConvexHull();
        }
    }
}