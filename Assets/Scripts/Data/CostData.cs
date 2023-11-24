using System;
using System.Collections.Generic;
using UnityEngine;



public class CostDataBuilder :DataBuilder<int>
{
    protected override int BuildData(List<string> s)
    {
        int cost = 0;
        SetValue(ref cost, "Cost");
        return cost;
    }

}