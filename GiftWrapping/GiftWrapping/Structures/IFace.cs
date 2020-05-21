using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IFace:ICell
    {
        IEnumerable<ICell> AdjacentCells { get; }
        IEnumerable<ICell> InnerCells { get; }
        void AddAdjacentCell(ICell cell);
    }
}