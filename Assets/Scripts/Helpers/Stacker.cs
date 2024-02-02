using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stacker<T>
{
    private Dictionary<T, int> dict = new Dictionary<T, int>();

    public void stack(T value)
    {
        if (dict.ContainsKey(value)) dict[value]++;
        else dict[value] = 1;
    }

    public void unstack(T value)
    {
        if (!dict.ContainsKey(value)) return;
        dict[value]--;
        if (dict[value] == 0) dict.Remove(value);
    }

    public HashSet<T> get()
    {
        return dict.Keys.ToHashSet();
    }
}
