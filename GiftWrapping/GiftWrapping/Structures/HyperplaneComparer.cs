using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class HyperplaneComparer:IEqualityComparer<Hyperplane>
    {
        private const double Accuracy = 6;
        public bool Equals(Hyperplane x, Hyperplane y)
        {
            return !ReferenceEquals(null, x) && x.Equals(y);
        }

        public int GetHashCode(Hyperplane obj)
        {
            int hc = obj.Dimension;
            for (int i = 0; i < obj.Dimension; ++i)
            {
                double sum = Math.Round(obj.Normal[i], 6, MidpointRounding.AwayFromZero);
                hc = unchecked(hc * 314159 + (int)(sum * Accuracy*10));
            }
            return hc;

        }
    }
}