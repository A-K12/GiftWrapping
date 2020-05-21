using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GiftWrapping.Helpers;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    internal class Iterator<T>:IEnumerable<T>
    {
        public IList<T> Objects { get; private set; }
        
        private bool[] _map;

        public Iterator(IList<T> obj)
        {
            Objects = new List<T>(obj);
            _map = new bool[obj.Count];
        }

        public void SetPoint(IList<T> obj)
        {
            Objects = obj;
            ClearMap();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if(_map[i]) continue;
                yield return Objects[i];
            }
        }
        public IEnumerator<T> GetExcludedEnumerator()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if(!_map[i]) continue;
                yield return Objects[i];
            }
        }
        public void ClearMap()
        {
            _map = new bool[Objects.Count];
        }
        public void ExcludeItem(T obj)
        {
            int index = Objects.IndexOf(obj);
            ExcludePoint(index);
        }

        public void ExcludePoint(int index)
        {
            _map[index] = true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}