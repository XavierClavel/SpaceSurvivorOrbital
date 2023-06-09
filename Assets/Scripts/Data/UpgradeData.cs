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

    public void Apply()
    {
        foreach (Effect effect in effects) effect.Apply();
    }

    static Dictionary<int, effectType> dictColumnToEffect;

    static List<string> firstLineValue = new List<string> {
        Vault.key.Name,
        Vault.key.upgrade.CostGreen,
        Vault.key.upgrade.CostOrange,
        Vault.key.upgrade.UpgradesEnabled,
        Vault.key.upgrade.UpgradesDisabled,

        Vault.key.upgrade.MaxHealth,
        Vault.key.upgrade.BaseSpeed,
        Vault.key.upgrade.DamageResistance,

        Vault.key.upgrade.BaseDamage,
        Vault.key.upgrade.AttackSpeed,
        Vault.key.upgrade.Range,
        Vault.key.upgrade.Cooldown,
        Vault.key.upgrade.Pierce,
        Vault.key.upgrade.Projectiles,
        Vault.key.upgrade.Spread,
        Vault.key.upgrade.AimingSpeed,
        Vault.key.upgrade.CriticalChance,
        Vault.key.upgrade.CriticalMultiplier,
        Vault.key.upgrade.Magazine,
        Vault.key.upgrade.MagazineCooldown,

        Vault.key.upgrade.MaxPurple,
        Vault.key.upgrade.MaxGreen,
        Vault.key.upgrade.MaxOrange,

        Vault.key.upgrade.AttractorRange,
        Vault.key.upgrade.AttractorForce,

        Vault.key.upgrade.Unlocks

};

    static List<effectType> effectsList = new List<effectType> {
        effectType.none,
        effectType.none,
        effectType.none,
        effectType.none,
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

        effectType.maxPurple,
        effectType.maxGreen,
        effectType.maxOrange,

        effectType.attractorRange,
        effectType.attractorForce,

        effectType.unlocks

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
        if (s == null || s.Count != firstLineValue.Count) return;

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

        effects.Add(new Effect(effect, operation, value));
    }


    static effectType columnNameToEffect(string columnName)
    {
        columnName = columnName.Trim();
        return (effectType)System.Enum.Parse(typeof(effectType), columnName, true);
    }


}
