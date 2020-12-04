using System;

class MinHeap
{
    MasterBoardContainer[] items;
    int size;
    bool min;

    public MinHeap(int maxSize, bool min)
    {
        items = new MasterBoardContainer[maxSize];
        this.min = min;
    }

    int parent(int index)
    {
        return ((index + 1) / 2) - 1;
    }
    int left(int index)
    {
        return (index * 2) + 1;
    }
    int right(int index)
    {
        return (index * 2) + 2;
    }
    void set(int index, MasterBoardContainer value)
    {
        items[index] = value;
    }

    void swap(int i, int j)
    {
        MasterBoardContainer item = items[i];
        set(i, items[j]);
        set(j, item);
    }

    void minHeapify(int index)
    {
        int l = left(index);
        int r = right(index);
        int smallest = index;
        if (l < size && (min ? items[l].value.CompareTo(items[smallest].value) < 0 : items[l].value.CompareTo(items[smallest].value) > 0))
        {
            smallest = l;
        }
        if (r < size && (min ? items[r].value.CompareTo(items[smallest].value) < 0 : items[r].value.CompareTo(items[smallest].value) > 0))
        {
            smallest = r;
        }
        if (smallest != index)
        {
            swap(index, smallest);

            minHeapify(smallest);
        }
    }

    public bool Empty()
    {
        return size == 0;
    }

    public void Insert(MasterBoardContainer item)
    {
        //set(size, item);
        size++;
        Decrease(size - 1, item);
    }
    public void Decrease(int index, MasterBoardContainer item)
    {
        set(index, item);
        while (index > 0 && (min ? items[parent(index)].value.CompareTo(item.value) > 0 : items[parent(index)].value.CompareTo(item.value) < 0))
        {
            swap(index, parent(index));
            index = parent(index);
        }
    }
    public MasterBoardContainer Minimum()
    {
        return items[0];
    }
    public MasterBoardContainer MinimumExtract()
    {
        if (size < 1)
        {
            throw new Exception("Heap is empty");
        }
        MasterBoardContainer min = items[0];
        set(0, items[size - 1]);
        items[size - 1] = null;
        size--;
        minHeapify(0);
        return min;
    }
}