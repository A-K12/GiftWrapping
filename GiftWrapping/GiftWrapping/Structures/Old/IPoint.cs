using System;
using System.Collections;

namespace GiftWrapping.Structures
{
    public interface IPoint:IComparable<IPoint>
    {
        double this[int index]
        {
            get;
            set;
        }

        int Dimension();

        double[] ToArray();
    }
}