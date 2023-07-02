using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : TemplateData
{
    public string name;
    public List<Effect> effects = new List<Effect>();

    static Dictionary<int, effectType> dictColumnToEffect;
    protected delegate void overrideMethod();

    static List<string> firstLineValue = new List<string> {
        Vault.playerParam_name,

        Vault.playerParam_maxHealth,
        Vault.playerParam_baseSpeed,
        Vault.playerParam_damageResistance,

        Vault.playerParam_baseDamage,
        Vault.playerParam_attackSpeed,
        Vault.playerParam_range,
        Vault.playerParam_cooldown,
        Vault.playerParam_pierce,
        Vault.playerParam_projectiles,
        Vault.playerParam_spread,
        Vault.playerParam_aimingSpeed,
        Vault.playerParam_criticalChance,
        Vault.playerParam_criticalMultiplier,
        Vault.playerParam_magazine,
        Vault.playerParam_magazineReloadTime,

        Vault.playerParam_toolPower,
        Vault.playerParam_toolRange,
        Vault.playerParam_toolSpeed,

        Vault.playerParam_maxPurple,
        Vault.playerParam_maxGreen,
        Vault.playerParam_maxOrange
    };

    static List<effectType> effectsList = new List<effectType> {
        effectType.none,

        effectType.maxHealth,
        effectType.baseSpeed,
        effectType.damageResistanceMultiplier,

        effectType.baseDamage,
        effectType.attackSpeed,
        effectType.range,
        effectType.bulletReloadTime,
        effectType.pierce,
        effectType.projectiles,
        effectType.spread,
        effectType.aimingSpeed,
        effectType.criticalChance,
        effectType.criticalMultiplier,
        effectType.magazine,
        effectType.magazineReloadTime,

        effectType.toolPower,
        effectType.toolRange,
        effectType.toolSpeed,

        effectType.maxPurple,
        effectType.maxGreen,
        effectType.maxOrange

    };

    protected static void SetOverrideValue<T>(List<string> s, Dictionary<int, int> mapper, int i, out T variable)
    {
        Debug.Log(i);
        i += firstLineValue.Count;
        Debug.Log(i);
        Debug.Log(mapper[i]);
        Debug.Log(typeof(T));
        Debug.Log(s[i]);

        Helpers.SetMappedValue(s, mapper, i, out variable);
    }




    protected static void OverrideInitialize(List<string> s, List<string> firstLineOverride = null)
    {
        List<string> list = firstLineValue.Union(firstLineOverride);
        InitializeMapping(s, list);
        Debug.Log(list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(mapper[i]);
        }

        dictColumnToEffect = new Dictionary<int, effectType>();
        for (int i = 1; i < firstLineValue.Count; i++)
        {
            dictColumnToEffect[i] = effectsList[i];
        }
    }

    protected void setEffects(List<string> s)
    {
        Helpers.SetMappedValue(s, mapper, 0, out name);
        for (int i = 1; i < s.Count; i++)
        {
            Debug.Log(i);
            if (mapper[i] >= firstLineValue.Count) continue;
            string str = s[mapper[i]].Trim();
            if (str == "") continue;
            Debug.Log(dictColumnToEffect[mapper[i]]);
            Process(str, dictColumnToEffect[mapper[i]]);
        }
    }

    public void Apply()
    {
        foreach (Effect effect in effects)
        {
            effect.Apply();
        }
        PlayerManager.CalculateDPS();
    }



    void Process(string value, effectType effect)
    {
        Debug.Log("processing value");
        operationType operation;
        if (value.Last() == '%')
        {
            if (value.First() == '+') operation = operationType.multiply;
            else if (value.First() == '-') operation = operationType.divide;
            else throw new System.ArgumentException($"{value} operation failed to parse");

            value.RemoveFirst();
            value.RemoveLast();

            float percentage = Helpers.parseString<float>(value);
            if (operation == operationType.multiply) percentage = 1 + percentage * 0.01f;
            else percentage = 1 - percentage * 0.01f;
            value = percentage.ToString();
        }
        else
        {
            if (value.First() == '+')
            {
                operation = operationType.add;
                value.RemoveFirst();
            }
            else if (value.First() == '-')
            {
                operation = operationType.substract;
                value.RemoveFirst();
            }
            else operation = operationType.assignation;
        }

        effects.Add(new Effect(effect, operation, value));
    }


    static effectType columnNameToEffect(string columnName)
    {
        columnName = columnName.Trim();
        return (effectType)System.Enum.Parse(typeof(effectType), columnName, true);
    }


}
