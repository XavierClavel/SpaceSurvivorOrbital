using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData
{
    public static void Initialize(List<string> columnNames)
    {

    }

    void Process(string value, effectType effect)
    {
        operationType operation;
        if (value.Last() == '%')
        {
            if (value.First() == '+') operation = operationType.multiply;
            else operation = operationType.divide;
            value.RemoveFirst();
            value.RemoveLast();
        }
        else
        {
            if (value.First() == '+') operation = operationType.add;
            else if (value.First() == '-') operation = operationType.substract;
            else operation = operationType.assignation;
        }
    }
}
