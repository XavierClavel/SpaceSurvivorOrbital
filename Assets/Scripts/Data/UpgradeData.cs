using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData : TemplateData
{
    public string name;
    public int costGreen;
    public int costOrange;
    public List<string> upgradesEnabled;
    public List<string> upgradesDisabled;
    public List<Effect> effects = new List<Effect>();

    static Dictionary<int, effectType> dictColumnToEffect;

    static List<string> firstLineValue = new List<string> {
        "Name",
        "CostGreen",
        "CostOrange",
        "UpgradesEnabled",
        "UpgradesDisabled",

        "MaxHealth",
        "BaseSpeed",
        "DamageResistance",

        "BaseDamage",
        "AttackSpeed",
        "Range",
        "Cooldown",
        "Pierce",
        "Projectiles",
        "Spread",
        "SpeedWhileAiming",
        "CriticalChance",
        "CriticalMultiplier",
        "Magazine",
        "MagazineReloadTime",

        "MaxPurple",
        "MaxGreen",
        "MaxOrange"
    };

    static List<effectType> effectsList = new List<effectType> {
        effectType.none,
        effectType.none,
        effectType.none,
        effectType.none,
        effectType.none,

        effectType.maxHealth,
        effectType.baseSpeed,
        effectType.damageResistance,

        effectType.WEAPONBaseDamage,
        effectType.WEAPONAttackSpeed,
        effectType.WEAPONRange,
        effectType.WEAPONCooldown,
        effectType.WEAPONPierce,
        effectType.WEAPONProjectiles,
        effectType.WEAPONSpread,
        effectType.WEAPONSpeedWhileAimingDecrease,
        effectType.WEAPONCriticalChance,
        effectType.WEAPONCriticalMultiplier,
        effectType.WEAPONMagazine,
        effectType.WEAPONMagazineReloadTime,

        effectType.CHARACTERMaxViolet,
        effectType.maxGreen,
        effectType.maxOrange

    };




    public static void Initialize(List<string> s)
    {
        InitializeMapping(s, firstLineValue);

        dictColumnToEffect = new Dictionary<int, effectType>();
        for (int i = 0; i < s.Count; i++)
        {
            dictColumnToEffect[mapper[i]] = effectsList[mapper[i]];
        }
    }

    public UpgradeData(List<string> s)
    {
        Helpers.SetMappedValue(s, mapper, 0, out name);
        Helpers.SetMappedValue(s, mapper, 1, out costGreen);
        Helpers.SetMappedValue(s, mapper, 2, out costOrange);
        Helpers.SetMappedValue(s, mapper, 3, out upgradesEnabled);
        Helpers.SetMappedValue(s, mapper, 4, out upgradesDisabled);

        for (int i = 5; i < s.Count; i++)
        {

            if (s[mapper[i]] == "") continue;
            Process(s[mapper[i]], dictColumnToEffect[i]);
        }


        DataManager.dictUpgrades.Add(name, this);
    }



    void Process(string value, effectType effect)
    {
        operationType operation;
        if (value.Last() == '%')
        {
            if (value.First() == '+') operation = operationType.multiply;
            else operation = operationType.divide;
            value.RemoveFirst();
            value.RemoveLast();
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
        switch (columnName)
        {
            case "maxHealth":
                return effectType.maxHealth;

            case "baseSpeed":
                return effectType.baseSpeed;

            case "damageResistance":
                return effectType.damageResistance;

            case "maxViolet":
                return effectType.CHARACTERMaxViolet;

            case "maxOrange":
                return effectType.maxOrange;

            case "maxGreen":
                return effectType.maxGreen;

            case "baseDamage":
                return effectType.WEAPONBaseDamage;

            case "attackSpeed":
                return effectType.WEAPONAttackSpeed;



            default:
                throw new System.ArgumentException($"\"{columnName}\" column is not recognized");
        }
    }


}
