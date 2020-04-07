using System.Collections.Generic;

namespace GiftWrapping.Structures
{
    public class Hyperface2d:IHyperface
    {
        private ICollection<Point> points;
        public int Dim { get; }

        public Hyperface2d()
        {
            points= new List<Point>();
        }

    }
}