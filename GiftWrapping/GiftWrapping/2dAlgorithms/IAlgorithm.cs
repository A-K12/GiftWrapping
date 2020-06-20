using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public interface IAlgorithm
    {
        IConvexHull FindConvexHull(IList<PlanePoint> points);
    }
}