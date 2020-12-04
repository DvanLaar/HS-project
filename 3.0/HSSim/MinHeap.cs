using System;

namespace HSSim
{
    internal class MinHeap
    {
        private readonly MasterBoardContainer[] _items;
        private int _size;
        private readonly bool _min;

        public MinHeap(int maxSize, bool min)
        {
            _items = new MasterBoardContainer[maxSize];
            _min = min;
        }

        private static int Parent(int index)
        {
            return ((index + 1) / 2) - 1;
        }

        private static int Left(int index)
        {
            return (index * 2) + 1;
        }

        private static int Right(int index)
        {
            return (index * 2) + 2;
        }

        private void Set(int index, MasterBoardContainer value)
        {
            _items[index] = value;
        }

        private void Swap(int i, int j)
        {
            var item = _items[i];
            Set(i, _items[j]);
            Set(j, item);
        }

        private void MinHeapify(int index)
        {
            while (true)
            {
                var l = Left(index);
                var r = Right(index);
                var smallest = index;
                if (l < _size && (_min ? _items[l].Value.CompareTo(_items[smallest].Value) < 0 : _items[l].Value.CompareTo(_items[smallest].Value) > 0))
                {
                    smallest = l;
                }
                if (r < _size && (_min ? _items[r].Value.CompareTo(_items[smallest].Value) < 0 : _items[r].Value.CompareTo(_items[smallest].Value) > 0))
                {
                    smallest = r;
                }

                if (smallest == index) return;
                Swap(index, smallest);

                index = smallest;
            }
        }

        public bool Empty()
        {
            return _size == 0;
        }

        public void Insert(MasterBoardContainer item)
        {
            Set(_size, item);
            _size++;
            Decrease(_size - 1, item);
        }

        private void Decrease(int index, MasterBoardContainer item)
        {
            Set(index, item);
            while (index > 0 && (_min ? _items[Parent(index)].Value.CompareTo(item.Value) > 0 : _items[Parent(index)].Value.CompareTo(item.Value) < 0))
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }
/*
        public MasterBoardContainer Minimum()
        {
            return _items[0];
        }
*/
        public MasterBoardContainer MinimumExtract()
        {
            if (_size < 1)
            {
                throw new Exception("Heap is empty");
            }
            var min = _items[0];
            Set(0, _items[_size - 1]);
            _items[_size - 1] = null;
            _size--;
            MinHeapify(0);
            return min;
        }
    }
}