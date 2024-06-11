
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CsvField: System.Attribute
{
    private string field;

    public CsvField(string field)
    {
        this.field = field;
    }

    public static Dictionary<string, string> getFields<T>(T variable)
    {
        Dictionary<string, string> dictVarToField = new Dictionary<string, string>();
        foreach (var property in variable.GetType().GetFields())
        {
            var attribute = (CsvField[])property.GetCustomAttributes(typeof(CsvField), false);
            if (attribute.Length == 0) continue;
            dictVarToField[property.Name] = attribute.First().field;
        }

        return dictVarToField;
    }
}
