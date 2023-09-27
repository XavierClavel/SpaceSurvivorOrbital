using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateData
{
    protected static Dictionary<int, int> mapper = new Dictionary<int, int>();
    static List<string> firstLine = new List<string>();
    protected List<string> row = new List<string>();

    public static void InitializeMapping(List<string> values, List<string> firstLineValue)
    {
        firstLine = firstLineValue;
        mapper = new Dictionary<int, int>();
        for (int i = 0; i < values.Count; i++) mapper[i] = columnToKey(values[i]);
    }

    public static int columnToKey(string columnName)
    {
        columnName = columnName.Trim();
        if (firstLine.IndexOf(columnName) == -1) throw new System.ArgumentException($"{columnName} not found");
        return firstLine.IndexOf(columnName);
    }

    protected void SetMappedValue<T>(int i, out T variable) {
        try {
            Helpers.SetMappedValue(row, mapper, i, out variable);
        } catch (System.Exception e) {
            string columnName = firstLine[mapper[i]];
            string key = row[mapper[0]];
            Debug.LogError($"Failed to read value in column \"{columnName}\" for key \"{key}\".");
            throw e;
        } 
    }

}
