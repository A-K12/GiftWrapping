using System;

namespace GiftWrapping.LinearEquations
{
    public class IndexMap
    {
        private int[] _indexes;

        public int Length { get; }

        public int this[Index i]
        {
            set => _indexes[i]= value;
            get => _indexes[i];
        }

        public IndexMap(int[] map)
        {
            Length = map.Length;
            _indexes = map;
        }

        public IndexMap(int length)
        {
            Length = length;
            _indexes = InitIndexMap(length);
        }

        private int[] InitIndexMap(int length)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = i;
            }

            return array;
        }
    }
}