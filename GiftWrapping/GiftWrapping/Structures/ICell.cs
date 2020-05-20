using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface ICell
    {
        int Dimension { get; }
        List<ICell> AdjacentCells { get; }
        Hyperplane Hyperplane { get; set; }

        IEnumerable<PlanePoint> GetPoints();
    }
}