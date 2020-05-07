﻿using System;
using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface ICell
    {
        int Dimension { get; }
        List<ICell> AdjacentCells { get; set; }
        Hyperplane Hyperplane { get; set; }
    }
}