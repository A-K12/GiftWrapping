using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface ICell:IEquatable<ICell>
    {
        int Dimension { get; }
        Hyperplane Hyperplane { get; set; }
        List<ICell> AdjacentCells { get; }
        ICollection<PlanePoint> GetPoints();
    }
}