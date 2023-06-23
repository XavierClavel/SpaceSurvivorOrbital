using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData : TemplateData
{
    public string name;
    public int maxHealth = 100;
    public float baseSpeed = 3.5f;
    public float damageResistance = 0f;

    static List<string> firstLineValue = new List<string> {
        "Name",
        "MaxHealth",
        "BaseSpeed",
        "DamageResistance"
    };

    public CharacterData(List<string> s)
    {
        if (s.Count != 4) throw new System.ArgumentOutOfRangeException();
        Helpers.SetMappedValue(s, mapper, 0, out name);
        Helpers.SetMappedValue(s, mapper, 1, out maxHealth);
        Helpers.SetMappedValue(s, mapper, 2, out baseSpeed);
        Helpers.SetMappedValue(s, mapper, 3, out damageResistance);

        CsvParser.dictCharacters.Add(name, this);
    }

    public static void Initialize(List<string> s)
    {
        InitializeMapping(s, firstLineValue);
    }

}
