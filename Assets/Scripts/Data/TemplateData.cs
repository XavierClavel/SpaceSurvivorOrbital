using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateData
{
    //Donne l'index réel à partir de l'index théorique
    protected static Dictionary<int, int> mapper = new Dictionary<int, int>();
    static List<string> firstLine = new List<string>();

    public static void InitializeMapping(List<string> values, List<string> firstLineValue)
    {
        firstLine = firstLineValue;
        mapper = new Dictionary<int, int>();
        foreach (string value in firstLine) Debug.Log(value);
        for (int i = 0; i < values.Count; i++) mapper[columnToKey(values[i])] = i;
    }

    public static int columnToKey(string columnName)
    {
        columnName = columnName.Trim();
        int result = firstLine.IndexOf(columnName, System.StringComparison.OrdinalIgnoreCase);
        if (result == -1) throw new System.ArgumentException($"{columnName} not found");
        return result;
    }

}
