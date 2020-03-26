using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PointIterator
    {
        protected readonly List<Point> _points;
        public IList<Point> Points => _points.AsReadOnly();

        private bool[] _map;

        public Point MinimumPoint { get; }

        public PointIterator(IList<Point> points)
        {
            if (!points.HaveSameDimension())
            {
                throw new ArgumentException("Vectors don't have same dimension");
            }

            _points = new List<Point>(points);
            _map = new bool[points.Count];
            MinimumPoint = points.FindMinimumPoint();
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if(_map[i]) continue;
                yield return _points[i];
            }
        }

        public void ExcludePoint(Point point)
        {
            int indexPoint = _points.IndexOf(point);
            _map[indexPoint] = true;
        }




    }
}