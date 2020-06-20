using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IFace:ICell
    {
        List<ICell> AdjacentCells { get; set; }
        List<ICell> InnerCells { get; set; }
        void AddAdjacentCell(ICell cell);
    }
}