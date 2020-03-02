using System;
using System.Collections;

namespace GiftWrapping.Structures
{
    public interface IPoint: IEquatable<IPoint>
    {
        double this[int index]
        {
            get;
            set;
        }

        int GetDimension();

        double[] ToArray();
    }
}