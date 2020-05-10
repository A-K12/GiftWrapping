using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IFace:ICell
    {
        List<ICell> InnerFaces { get; }
    }
}