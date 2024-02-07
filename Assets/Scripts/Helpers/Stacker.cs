using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Stacker<T>
{
    private Dictionary<T, int> dict = new Dictionary<T, int>();
    private UnityAction<T> onStartStacking = null;
    private UnityAction<T> onStopStacking = null;
    
    public Stacker<T> addOnStartStackingEvent(UnityAction<T> action)
    {
        onStartStacking = action;
        return this;
    }

    public Stacker<T> addOnStopStackingEvent(UnityAction<T> action)
    {
        onStopStacking = action;
        return this;
    }


    public void stack(T value)
    {
        if (dict.ContainsKey(value)) dict[value]++;
        else
        {
            dict[value] = 1;
            onStartStacking?.Invoke(value);
        }
    }

    public void unstack(T value)
    {
        if (!dict.ContainsKey(value)) return;
        dict[value]--;
        if (dict[value] == 0)
        {
            dict.Remove(value);
            onStopStacking?.Invoke(value);
        }
    }

    public HashSet<T> get()
    {
        return dict.Keys.ToHashSet();
    }
}

public class SingleStacker
{
    private int amount = 0;
    private UnityAction onStartStacking = null;
    private UnityAction onStopStacking = null;
    
    public SingleStacker addOnStartStackingEvent(UnityAction action)
    {
        onStartStacking = action;
        return this;
    }

    public SingleStacker addOnStopStackingEvent(UnityAction action)
    {
        onStopStacking = action;
        return this;
    }

    public void stack()
    {
        if (amount == 0)
        {
            onStartStacking?.Invoke();
        }

        amount++;
    }

    public void unstack()
    {
        amount--;
        if (amount == 0)
        {
            onStopStacking?.Invoke();
        }
    }

}
