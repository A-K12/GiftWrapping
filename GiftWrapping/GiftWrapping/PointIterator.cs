using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    internal class PointIterator
    {
        public IList<Point> Points { get; private set; }
        
        private bool[] _map;

        public PointIterator(IList<Point> points)
        {
            Points = new List<Point>(points);
            _map = new bool[points.Count];
        }

        public void SetPoint(IList<Point> points)
        {
            Points = points;
            ClearMap();
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if(_map[i]) continue;
                yield return Points[i];
            }
        }

        public void ClearMap()
        {
            _map = new bool[Points.Count];
        }
        public void ExcludePoint(Point point)
        {
            int indexPoint = Points.IndexOf(point);
            ExcludePoint(indexPoint);
        }

        public void ExcludePoint(int index)
        {
            _map[index] = true;
        }
    }
}