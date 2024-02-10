using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectData
{
    static Dictionary<string, effectType> dictKeyToEffect = new Dictionary<string, effectType>
    {
        {Vault.key.upgrade.MaxHealth, effectType.maxHealth},
        {Vault.key.upgrade.BaseSpeed, effectType.baseSpeed},
        {Vault.key.upgrade.DamageResistance, effectType.damageResistanceMultiplier},

        {Vault.key.upgrade.BaseDamage, effectType.baseDamage},
        {Vault.key.upgrade.AttackSpeed, effectType.attackSpeed},
        {Vault.key.upgrade.Range, effectType.range},
        {Vault.key.upgrade.Cooldown, effectType.bulletReloadTime},
        {Vault.key.upgrade.Pierce, effectType.pierce},
        {Vault.key.upgrade.Projectiles, effectType.projectiles},
        {Vault.key.upgrade.Spread, effectType.spread},
        {Vault.key.upgrade.AimingSpeed, effectType.aimingSpeed},
        {Vault.key.upgrade.CriticalChance, effectType.criticalChance},
        {Vault.key.upgrade.CriticalMultiplier, effectType.criticalMultiplier},
        {Vault.key.upgrade.Magazine, effectType.magazine},
        {Vault.key.upgrade.MagazineCooldown, effectType.magazineReloadTime},

        {Vault.key.upgrade.MaxPurple, effectType.maxPurple},
        {Vault.key.upgrade.MaxGreen, effectType.maxGreen},
        {Vault.key.upgrade.MaxOrange, effectType.maxOrange},

        {Vault.key.upgrade.AttractorRange, effectType.attractorRange},
        {Vault.key.upgrade.AttractorForce, effectType.attractorForce},

        {Vault.key.Sprite, effectType.weapon},

        {Vault.key.upgrade.Unlocks, effectType.unlocks},
        
        {"Element", effectType.effect},
        
        {"BoolA", effectType.boolA},
        {"BoolB", effectType.boolB},
        {"BoolC", effectType.boolC},
        {"BoolD", effectType.boolD},
        {"IntA", effectType.intA},
        {"IntB", effectType.intB},
        {"FloatA",effectType.floatA},
        {"FloatB",effectType.floatB},
        {"ElementA", effectType.elementA},
    };


    protected Dictionary<string, string> dictColumnToValue;

    protected static List<string> InitializeColumnTitles(List<string> s)
    {
        List<string> columnTitles = new List<string>();
        for (int i = 0; i < s.Count; i++)
        {
            columnTitles.Add(s[i].Trim());
        }
        return columnTitles;
    }

    protected void SetDictionary(List<string> columnTitles, List<string> s)
    {
        dictColumnToValue = new Dictionary<string, string>();

        for (int i = 0; i < s.Count; i++)
        {
            dictColumnToValue[columnTitles[i]] = s[i];
        }
    }

    protected T getValue<T>(string key, T defaultValue)
    {
        string value = dictColumnToValue[key];
        if (string.IsNullOrEmpty(value)) return defaultValue;
        try
        {
            return Helpers.parseString<T>(dictColumnToValue[key]);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse value in column \"{key}\".");
            return defaultValue;
        }
    }

    protected void SetValue<T>(ref T variable, string key)
    {
        string value = dictColumnToValue[key];
        if (string.IsNullOrEmpty(value)) return;
        try
        {
            variable = Helpers.parseString<T>(dictColumnToValue[key]);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse value in column \"{key}\".");
        }

    }

    protected void TrySetValue<T>(ref T variable, string key)
    {
        if (!dictColumnToValue.ContainsKey(key)) return;
        SetValue(ref variable, key);
    }



    protected void ProcessEffects(List<string> columnTitles, List<string> s, ref List<Effect> effects)
    {
        for (int i = 0; i < s.Count; i++)
        {
            string str = s[i].Trim();
            if (str != "" && dictKeyToEffect.ContainsKey(columnTitles[i]))
            {
                effects.Add(Process(str, dictKeyToEffect[columnTitles[i]]));
            }
        }
    }


    Effect Process(string value, effectType effect)
    {
        operationType operation;
        if (value.Last() == '%')
        {
            if (value.First() == '+') operation = operationType.multiply;
            else if (value.First() == '-') operation = operationType.divide;
            else throw new System.ArgumentException($"{value} operation failed to parse");

            value = value.RemoveFirst();
            value = value.RemoveLast();
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
                value = value.RemoveFirst();
            }
            else if (value.First() == '-')
            {
                operation = operationType.substract;
                value = value.RemoveFirst();
            }
            else operation = operationType.assignation;
        }

        return new Effect(effect, operation, value);
    }
    
    
    
    

}
