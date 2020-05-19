using System;
using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class Edge2d
    {
        
        public Point[] Points { get;  }

        public Edge2d(Point p1, Point p2)
        {
            if(p1 == p2) throw new ArgumentException("Points are equal.");
            Points = new[] {p1, p2};
        }

        public Edge2d()
        {
            Points = new Point[2];
        }

        public void SetPoints(Point p1, Point p2)
        {
            Points[0] = p1;
            Points[1] = p2;
        }

        public override int GetHashCode()
        {
            int res = 0;
            for (int i = 0; i < Points.Length; i++)
                res += Points[i].GetHashCode();
            return res;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge2d)obj);
        }

        public bool Equals(Edge2d obj)
        {
            return obj != null && Points.All(obj.Points.Contains);
        }

        public object Clone()
        {
            return new Edge2d(Points[0], Points[1]);
        }

        public static IEnumerable<Edge2d> GetEdges(ConvexHull2d convexHull)
        {
            IList<PlanePoint> planePoints = convexHull.GetPoints().ToList();

            Edge2d tempEdge = new Edge2d(planePoints[^1].OriginalPoint, planePoints[0].OriginalPoint);
            yield return tempEdge;
            for (int i = 0; i < planePoints.Count-1; i++)
            {
                //tempEdge.SetPoints(planePoints[i].OriginalPoint, planePoints[i+1].OriginalPoint);
                yield return new Edge2d(planePoints[i].OriginalPoint, planePoints[i + 1].OriginalPoint);
            }
        }
    }
}