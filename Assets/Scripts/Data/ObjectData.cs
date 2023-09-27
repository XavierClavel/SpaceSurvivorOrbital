using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : TemplateData
{

    static List<string> firstLineValue = new List<string> {
        "Name",
        "MaxHealth",
        "BaseDamage",
        "BaseSpeed",
        "DamageResistance"
    };

    public string name;
    public int maxHealth = 100;
    public float baseSpeed = 3.5f;
    public float damageResistance = 0f;
    public Vector2Int baseDamage = new Vector2Int(5,5);

    public ObjectData(List<string> s)
    {
        if (s == null || s.Count != firstLineValue.Count) return;

        Helpers.SetMappedValue(s, mapper, 0, out name);
        Helpers.SetMappedValue(s, mapper, 1, out maxHealth);
        Helpers.SetMappedValue(s, mapper, 2, out baseDamage);
        Helpers.SetMappedValue(s, mapper, 3, out baseSpeed);
        Helpers.SetMappedValue(s, mapper, 4, out damageResistance);

        if (DataManager.dictObjects.ContainsKey(name)) {
            throw new System.ArgumentException($"Key {name} already used.")
        }
        
        DataManager.dictObjects.Add(name, this);
    }

    public static void Initialize(List<string> s)
    {
        InitializeMapping(s, firstLineValue);
    }

}
