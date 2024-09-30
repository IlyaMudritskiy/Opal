using System;
using System.Collections.Generic;

public sealed class FixedSizeBuffer<T>
{
    public List<T> Buffer;
    private int _maxSize;

    // Empty constructor
    public FixedSizeBuffer()
    {
        Buffer = new List<T>();
        _maxSize = int.MaxValue; // Default to unlimited size
    }

    // Constructor with maxSize
    public FixedSizeBuffer(int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentException("Max size must be greater than zero", nameof(maxSize));

        Buffer = new List<T>(maxSize);
        _maxSize = maxSize;
    }

    public void Add(T item)
    {
        if (_maxSize != int.MaxValue && Buffer.Count >= _maxSize)
        {
            Buffer.RemoveAt(0); // Remove oldest element if buffer is full
        }
        Buffer.Add(item);
    }

    public void Add(List<T> items)
    {
        foreach (T item in items)
        {
            Add(item);
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
            return Buffer[index];
        }
    }

    public int Count => Buffer.Count;

    public bool IsEmpty => Count == 0;

    public bool IsFull => _maxSize != int.MaxValue && Count >= _maxSize;

    // Method to set the maximum size
    public void SetSize(int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentException("Max size must be greater than zero", nameof(maxSize));

        _maxSize = maxSize;
        if (Buffer.Count > _maxSize)
        {
            // Remove excess elements if buffer exceeds new max size
            Buffer.RemoveRange(_maxSize, Buffer.Count - _maxSize);
        }
    }
}
