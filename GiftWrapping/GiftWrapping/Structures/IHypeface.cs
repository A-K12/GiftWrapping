using System;

namespace GiftWrapping.Structures
{
    public interface IHypeface
    {
        public IHypeface NeighboringFace { get; set; }
        public IHypeface ParentFace { get; set; }
        int Dim { get; }
    }
}