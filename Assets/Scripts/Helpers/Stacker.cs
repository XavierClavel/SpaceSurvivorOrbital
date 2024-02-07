using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

public class Stacker<T>
{
    private Dictionary<T, int> dict = new Dictionary<T, int>();
    private List<UnityAction<T>> onStartStacking = new List<UnityAction<T>>();
    private List<UnityAction<T>> onStopStacking = new List<UnityAction<T>>();
    
    public Stacker<T> addOnStartStackingEvent(UnityAction<T> action)
    {
        onStartStacking.Add(action);
        return this;
    }

    public Stacker<T> addOnStopStackingEvent(UnityAction<T> action)
    {
        onStopStacking.Add(action);
        return this;
    }


    public void stack(T value)
    {
        if (dict.ContainsKey(value)) dict[value]++;
        else
        {
            dict[value] = 1;
            onStartStacking.ForEach(it => it.Invoke(value));
        }
    }

    public void unstack(T value)
    {
        if (!dict.ContainsKey(value)) return;
        dict[value]--;
        if (dict[value] == 0)
        {
            dict.Remove(value);
            onStopStacking.ForEach(it => it.Invoke(value));
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
    private List<UnityAction> onStartStacking = new List<UnityAction>();
    private List<UnityAction> onStopStacking = new List<UnityAction>();
    private Dictionary<int, UnityAction> onAboveThreshold = new Dictionary<int, UnityAction>();
    private Dictionary<int, UnityAction> onBelowThreshold = new Dictionary<int, UnityAction>();
    
    public SingleStacker addOnStartStackingEvent(UnityAction action)
    {
        onStartStacking.Add(action);
        return this;
    }

    public SingleStacker addOnStopStackingEvent(UnityAction action)
    {
        onStopStacking.Add(action);
        return this;
    }
    
    public SingleStacker addThresholdAction(int threshold, UnityAction aboveAction, UnityAction belowAction = null)
    {
        onAboveThreshold[threshold] = aboveAction;
        if (belowAction != null)
        {
            onBelowThreshold[threshold] = belowAction;
        }
        return this;
    }

    public void stack()
    {
        if (amount == 0)
        {
            onStartStacking.ForEach(it => it.Invoke());
        }

        amount++;

        foreach (var v in onAboveThreshold)
        {
            if (v.Key != amount) { continue;}
            v.Value.Invoke();
        }
    }

    public void unstack()
    {
        amount--;
        if (amount == 0)
        {
            onStopStacking.ForEach(it => it.Invoke());
        }
        
        foreach (var v in onBelowThreshold)
        {
            if (v.Key != amount - 1) { continue;}
            v.Value.Invoke();
        }
    }

    public void reset()
    {
        amount = 0;
    }

}
