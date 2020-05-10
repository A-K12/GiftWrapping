using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping.Helpers
{
    public static class ConvexHullHelper
    {
        public static ConvexHull ToConvexHull2d(this IList<Point> points)
        {
            ConvexHull result = new ConvexHull(2);
            for (int i = 1; i < points.Count; i++)
            {
                Edge2d edge = new Edge2d();
                edge.Points[0] = points[i-1];
                edge.Points[1] = points[i];
                result.InnerFaces.Add(edge);
            }

            return result;
        }
    }
}