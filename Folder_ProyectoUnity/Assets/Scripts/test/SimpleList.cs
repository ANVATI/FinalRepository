public class SimpleList<T>
{
    private T[] array;
    private int length;
    private int capacity;

    public SimpleList()
    {
        capacity = 4;
        array = new T[capacity];
        length = 0;
    }

    public void Add(T item)
    {
        if (length == capacity)
        {
            T[] newArray = new T[capacity * 2];
            for (int i = 0; i < length; i++)
            {
                newArray[i] = array[i];
            }
            array = newArray;
            capacity *= 2;
        }
        array[length] = item;
        length++;
    }


    public T Get(int index)
    {
        if (index < 0 || index >= length)
        {
            throw new System.IndexOutOfRangeException("Index out of range");
        }
        return array[index];
    }

    public int Length => length;

    public bool Contains(T item)
    {
        for (int i = 0; i < length; i++)
        {
            if (array[i].Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    public void Remove(T item)
    {
        int index = -1;
        for (int i = 0; i < length; i++)
        {
            if (array[i].Equals(item))
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            for (int i = index; i < length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            length--;
        }
    }

    public void Clear()
    {
        array = new T[capacity];
        length = 0;
    }
}
