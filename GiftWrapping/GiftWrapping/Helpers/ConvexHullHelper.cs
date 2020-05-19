using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping.Helpers
{
    public static class ConvexHullHelper
    {
        public static ConvexHull2d ToConvexHull2d(this IList<Point> points)
        {
            ConvexHull2d result = new ConvexHull2d();
            foreach (Point point in points)
            {
                result.AddPoint(new PlanePoint(point));
            }
            return result;
        }
    }
}