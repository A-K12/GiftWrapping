using System;

namespace GiftWrapping.LinearEquations
{
    public class IndexMap
    {
        private int[] _indexes;

        public int this[Index i]
        {
            set => _indexes[i]= value;
            get => _indexes[i];
        }

        public IndexMap(int length)
        {
            _indexes = InitIndexMap(length);
        }

        private int[] InitIndexMap(int length)
        {
            var array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = i;
            }

            return array;
        }
    }
}