using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class SimplexCreator:IAlgorithm
    {
        public IConvexHull FindConvexHull(IList<PlanePoint> points)
        {
            int dim = points[0].Dim;
            if (dim == 2)
            {
                return new ConvexHull2d(points);
            }
            ConvexHull convexHull = new ConvexHull(dim);
            PlanePoint[] planePoints = new PlanePoint[points.Count - 1];
            for (int i = 0; i < points.Count; i++)
            {
                int k = 0;
                for (int j = 0; j < points.Count; j++)
                {
                    if (j == i) continue;
                    planePoints[k++] = points[j];
                }
                Hyperplane hyperplane = Hyperplane.Create(planePoints);
                List<PlanePoint> convertPoints =
                    planePoints.Select((point => hyperplane.ConvertPoint(point))).ToList();
                Face newFace = new Face(hyperplane);
                newFace.ConvexHull = FindConvexHull(convertPoints);
                foreach (ICell f in newFace.ConvexHull.Cells)
                {
                    newFace.AdjacentCells.Add((IFace)f);
                    ((IFace)f).AdjacentCells.Add(newFace);
                }

                convexHull.AddInnerCell(newFace);
            }

            return convexHull;
        }
    }
}