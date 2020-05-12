using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IPointFace:ICell
    {
        void AddPoint(Point point);

        Point this[int i]
        {
            get;
        }
    }
}