using System;

public class CustomList<T>
{
    private T[] array;
    private int length;
    private int capacity;

    public CustomList()
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
            throw new IndexOutOfRangeException("Index out of range");
        }
        return array[index];
    }

    public int Length
    {
        get { return length; }
    }
}
