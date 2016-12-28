using System;
using System.Collections.Generic;

class MyList<T> : ICollection<T>
{
    public List<T> innerList;
    public int Count { get { return innerList.Count; } }
    public bool IsReadOnly { get { return false; } }

    public MyList()
    {
        innerList = new List<T>();
    }

    public void Add(T item)
    {
        if (innerList.TrueForAll(i => !i.Equals(item)))
            innerList.Add(item);
    }
    public bool Remove(T item)
    {
        return innerList.Remove(item);
    }
    public IEnumerator<T> GetEnumerator()
    {
        return innerList.GetEnumerator();
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return innerList.GetEnumerator();
    }
    public void CopyTo(T[] array, int index)
    {
        innerList.CopyTo(array, index);
    }
    public bool Contains(T item)
    {
        return innerList.Contains(item);
    }
    public void Clear()
    {
        innerList.Clear();
    }

}