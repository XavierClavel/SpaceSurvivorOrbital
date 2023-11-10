using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData
{
    public string key;
    public int baseCost;
    public int increment;
    public int minSpending;
    public int maxSpending;
}

public class SpawnDataBuilder : DataBuilder<SpawnData>
{

    protected override SpawnData BuildData(List<string> s)
    {
        SpawnData value = new SpawnData();

        SetValue(ref value.key, "Key");
        
        SetValue(ref value.baseCost, "BaseCost");
        SetValue(ref value.increment, "Increment");
        SetValue(ref value.minSpending, "MinSpending");
        SetValue(ref value.maxSpending, "MaxSpending");

        DataManager.dictDifficulty[value.key] = value;

        return value;
    }

}
