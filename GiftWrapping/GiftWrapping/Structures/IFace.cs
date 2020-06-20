using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IFace:ICell
    {
        IConvexHull ConvexHull { get; set; }
    }
}