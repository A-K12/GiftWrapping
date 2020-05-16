using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public interface IPointFace:ICell
    {
        void AddPoint(PlanePoint point);

        PlanePoint this[int i]
        {
            get;
        }
    }
}