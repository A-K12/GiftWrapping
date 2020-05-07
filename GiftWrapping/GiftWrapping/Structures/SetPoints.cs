using System.Collections.Generic;
using System.Linq;

namespace GiftWrapping.Structures
{
    public class SetPoints
    {
        private IList<Point> Points { get; set; }

        private int[] mask;

        public IEnumerable<double> this[int i]
        {
            get { return mask.Select(index => Points[i][index]); }
        }
    }
}