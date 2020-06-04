using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class GiftWrapping2d:IAlgorithm
    {
        public IFace FindConvexHull(IList<PlanePoint> points)
        {
            Stopwatch sp1 = new Stopwatch();
            sp1.Start();
            if (points.Count == 3)
            {
                return new ConvexHull2d(points);
            }
            List<PlanePoint> hullPoints = new List<PlanePoint>();
            PlanePoint first = points.Min();
            Vector currentVector = new Vector(new double[] { 0, -1 });
            PlanePoint currentPlanePoint = first;
            do
            {
                hullPoints.Add(currentPlanePoint);
                double maxCos = double.MinValue;
                double maxLen = double.MinValue;
                PlanePoint next = currentPlanePoint;
                Vector maxVector = currentVector;
                foreach (PlanePoint point in points)
                {
                    if (currentPlanePoint == point) continue;
                    Vector newVector = Point.ToVector(currentPlanePoint, point);
                    double newCos = currentVector * newVector;
                    newCos /= newVector.Length * currentVector.Length;
                    if (Tools.GT(newCos, maxCos))
                    {
                        maxCos = newCos;
                        next = point;
                        maxLen = Point.Length(currentPlanePoint, next);
                        maxVector = newVector;
                    }
                    else if (Tools.EQ(maxCos, newCos))
                    {
                        double dist = Point.Length(currentPlanePoint, point);
                        if (Tools.LT(maxLen, dist))
                        {
                            next = point;
                            maxVector = newVector;
                            maxLen = dist;
                        }
                    }
                }

                currentPlanePoint = next;
                currentVector = maxVector;
            } while (first != currentPlanePoint);

            ConvexHull2d hull = new ConvexHull2d(hullPoints);
            sp1.Stop();
            long time = sp1.ElapsedMilliseconds;
            return hull;
        }
    }
}