using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerData : EffectData
{
    public string name;
    public interactorStats stats = new interactorStats();


    static List<string> columnTitles = new List<string>();

    public static void Initialize(List<string> s)
    {
        columnTitles = InitializeColumnTitles(s);
    }

    public PowerData(List<string> s, selectorType type)
    {
        if (s == null || s.Count != columnTitles.Count) return;

        SetDictionary(columnTitles, s);

        SetValue(ref name, Vault.key.Name);
        SetValue(ref stats.baseDamage, Vault.key.upgrade.BaseDamage);
        SetValue(ref stats.attackSpeed, Vault.key.upgrade.AttackSpeed);
        SetValue(ref stats.range, Vault.key.upgrade.Range);
        SetValue(ref stats.cooldown, Vault.key.upgrade.Cooldown);
        SetValue(ref stats.pierce, Vault.key.upgrade.Pierce);
        SetValue(ref stats.projectiles, Vault.key.upgrade.Projectiles);
        SetValue(ref stats.spread, Vault.key.upgrade.Spread);
        SetValue(ref stats.speedWhileAiming, Vault.key.upgrade.AimingSpeed);
        SetValue(ref stats.criticalChance, Vault.key.upgrade.CriticalChance);
        SetValue(ref stats.criticalMultiplier, Vault.key.upgrade.CriticalChance);
        SetValue(ref stats.magazine, Vault.key.upgrade.Magazine);
        SetValue(ref stats.magazineReloadTime, Vault.key.upgrade.MagazineCooldown);
        SetValue(ref stats.dps, "DPS");

        stats.CalculateDPS();

        DataManager.dictPowers.Add(name,stats);

        if (type == selectorType.weapon)
        {
            weapon currentInteractor = (weapon)System.Enum.Parse(typeof(weapon), name);
            DataManager.dictWeapons.Add(currentInteractor, this);
        }
        else if (type == selectorType.tool)
        {
            //tool currentInteractor = (tool)System.Enum.Parse(typeof(tool), name);
            //DataManager.dictTools.Add(currentInteractor, this);
        }

    }

}
