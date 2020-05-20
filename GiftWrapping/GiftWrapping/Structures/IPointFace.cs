using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GiftWrapping.Structures
{
    public interface IPointFace:ICell
    {
        IList<PlanePoint> Points { get; }
    }
}