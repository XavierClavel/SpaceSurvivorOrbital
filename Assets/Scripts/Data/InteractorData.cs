using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorStats
{
    public Vector2Int baseDamage;
    public int attackSpeed;
    public float range;

    public float cooldown;
    public float magazineReloadTime;

    public float criticalChance;
    public float criticalMultiplier;

    public int magazine;
    public int projectiles;
    public float spread;

    public int pierce;
    public float speedWhileAiming;

    public int dps;

    public void CalculateDPS()
    {
        if (cooldown == 0f) dps = baseDamage.Mean();
        else dps = baseDamage.Mean(); //(int)((float)baseDamage.Mean() / cooldown);
    }

}

public class InteractorData : EffectData
{
    public string name;
    public InteractorStats interactorStats = new InteractorStats();


    static List<string> columnTitles = new List<string>();

    public static void Initialize(List<string> s)
    {
        columnTitles = InitializeColumnTitles(s);
    }

    public InteractorData(List<string> s, selectorType type)
    {
        if (s == null || s.Count != columnTitles.Count) return;

        SetDictionary(columnTitles, s);

        SetValue(ref name, Vault.key.Name);
        SetValue(ref interactorStats.baseDamage, Vault.key.upgrade.BaseDamage);
        SetValue(ref interactorStats.attackSpeed, Vault.key.upgrade.AttackSpeed);
        SetValue(ref interactorStats.range, Vault.key.upgrade.Range);
        SetValue(ref interactorStats.cooldown, Vault.key.upgrade.Cooldown);
        SetValue(ref interactorStats.pierce, Vault.key.upgrade.Pierce);
        SetValue(ref interactorStats.projectiles, Vault.key.upgrade.Projectiles);
        SetValue(ref interactorStats.spread, Vault.key.upgrade.Spread);
        SetValue(ref interactorStats.speedWhileAiming, Vault.key.upgrade.AimingSpeed);
        SetValue(ref interactorStats.criticalChance, Vault.key.upgrade.CriticalChance);
        SetValue(ref interactorStats.criticalMultiplier, Vault.key.upgrade.CriticalChance);
        SetValue(ref interactorStats.magazine, Vault.key.upgrade.Magazine);
        SetValue(ref interactorStats.magazineReloadTime, Vault.key.upgrade.MagazineCooldown);
        SetValue(ref interactorStats.dps, "DPS");

        interactorStats.CalculateDPS();

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
