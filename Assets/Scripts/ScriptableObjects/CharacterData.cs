using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData
{
    public int maxHealth = 100;
    public float baseSpeed = 3.5f;
    public float damageResistance = 0f;

    public CharacterData(List<string> s)
    {
        Debug.Log(s.Count);
        if (s.Count != 4) throw new System.ArgumentOutOfRangeException();

        maxHealth = int.Parse(s[1]);
        baseSpeed = float.Parse(s[2]);
        damageResistance = float.Parse(s[3]);

        CsvParser.dictCharacters.Add(s[0], this);
    }

}
