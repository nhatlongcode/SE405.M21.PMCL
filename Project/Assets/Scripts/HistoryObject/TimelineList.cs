using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelfBalancedTree;
[Serializable]
public class StackList<T> : List<T>
{
    public T Peek() { return this[Count - 1]; }
    public void Pop()
    {
        RemoveAt(Count - 1);
    }
    public void Push(T item)
    {
        Add(item);
    }
}

[Serializable]
public class TimelineList<T> where T : IEquatable<T>
{
    public StackList<Tuple<float, T>> Timeline = new StackList<Tuple<float, T>>();

    public void Add(float time, T action)
    {
        while (Timeline.Count > 0 && Timeline.Peek().Item1 > time)
            Timeline.Pop();

        if (Timeline.Count > 0 && Timeline.Peek().Item2.Equals(action))
        {
            return;
        }
        else
        {
            Timeline.Push(Tuple.Create(time, action));
        }
    }

    public T GetAction(float time)
    {
        int l = 0;
        int r = Timeline.Count - 1;
        while (l > r)
        {
            int m = (l + r) / 2;
            if (Timeline[m].Item1 < time)
                l = m + 1;
            else
                r = m - 1;
        }
        if (r >= 0)
            return Timeline[r].Item2;
        else
            return default;
    }
}
