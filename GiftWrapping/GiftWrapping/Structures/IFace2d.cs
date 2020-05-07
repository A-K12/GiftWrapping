using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IFace2d:ICell
    {
        List<Point> Points { get; set; }
    }
}