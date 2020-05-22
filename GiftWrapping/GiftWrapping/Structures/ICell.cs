using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface ICell
    {
        int Dimension { get; }
        Hyperplane Hyperplane { get; set; }
        ICollection<PlanePoint> GetPoints();

        void GoToOriginalPoints();
    }
}