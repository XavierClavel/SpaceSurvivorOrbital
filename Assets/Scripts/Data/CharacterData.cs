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
        Vault.key.Name,
        Vault.key.upgrade.MaxHealth,
        Vault.key.upgrade.BaseSpeed,
        Vault.key.upgrade.DamageResistance
    };

    public CharacterData(List<string> s)
    {
        Helpers.SetMappedValue(s, mapper, 0, out name);
        Helpers.SetMappedValue(s, mapper, 1, out maxHealth);
        Helpers.SetMappedValue(s, mapper, 2, out baseSpeed);
        Helpers.SetMappedValue(s, mapper, 3, out damageResistance);

        character currentCharacter = (character)System.Enum.Parse(typeof(character), name);
    }

    public static void Initialize(List<string> s)
    {
        InitializeMapping(s, firstLineValue);
    }

}
