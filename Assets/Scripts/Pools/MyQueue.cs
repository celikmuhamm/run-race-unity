using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Todo : To create a queue that also has the ability to remove object from requested index;
 * @date : 5/21/2020
 * @author : Safa Celik
 */
 
public class MyQueue<T>
{
    private LinkedList<T> _objectList;
    private int _queueSize;
    public MyQueue()
    {
        _objectList = new LinkedList<T>();
    }
    public int Count
    {
        get
        {
            return _queueSize;
        }
    }

    public LinkedList<T> Objects
    {
        get
        {
            return _objectList;
        }
    }
    public T Dequeue()
    {
        T firstOut = _objectList.First.Value;
        _objectList.RemoveFirst();
        _queueSize = _objectList.Count;
        return firstOut;
    }

    public void Enqueue(T inObject)
    {
        _objectList.AddLast(inObject);
        _queueSize = _objectList.Count;
    }

    public void RemoveObject(T node)
    {
        _objectList.Remove(node);
        _queueSize = _objectList.Count;
    }

}
