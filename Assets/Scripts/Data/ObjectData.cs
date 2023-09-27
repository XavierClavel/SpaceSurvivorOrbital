using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : TemplateData
{

    static Dictionary<int, string> columns = new Dictionary<int, string> 
    {
        {0, "Name"},
        {1, "MaxHealth"},
        {2, "BaseDamage"},
        {3, "BaseSpeed"},
        {4, "DamageResistance"},
    };   

    public string name;
    public int maxHealth = 100;
    public float baseSpeed = 3.5f;
    public float damageResistance = 0f;
    public Vector2Int baseDamage = new Vector2Int(5,5);

    public ObjectData(List<string> s)
    {
        if (s == null || s.Count != columns.Values.ToList().Count) return;

        row = s.Copy();

        SetMappedValue(0, out name);
        SetMappedValue(1, out maxHealth);
        SetMappedValue(2, out baseDamage);
        SetMappedValue(3, out baseSpeed);
        SetMappedValue(4, out damageResistance);

        if (DataManager.dictObjects.ContainsKey(name)) {
            throw new System.ArgumentException($"Key {name} already used.");
        }
        
        DataManager.dictObjects.Add(name, this);
    }

    public static void Initialize(List<string> s)
    {
        InitializeMapping(s, columns.Values.ToList());
    }

}
