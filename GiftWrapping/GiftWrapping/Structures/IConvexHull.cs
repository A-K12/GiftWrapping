using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GiftWrapping.Structures
{
    public interface IConvexHull
    {
        public int Dimension { get; }
        List<ICell> Faces { get; }
        ICollection<PlanePoint> GetPoints();
    }
}