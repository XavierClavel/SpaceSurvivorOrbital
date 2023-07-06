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

public class InteractorData : TemplateData
{
    public string name;
    public InteractorStats interactorStats = new InteractorStats();

    static List<string> firstLineValue = new List<string> {
        "Name",
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
        "DPS"
    };

    public static void Initialize(List<string> s)
    {
        InitializeMapping(s, firstLineValue);
    }

    public InteractorData(List<string> s)
    {
        Helpers.SetMappedValue(s, mapper, 0, out name);
        Helpers.SetMappedValue(s, mapper, 1, out interactorStats.baseDamage);
        Helpers.SetMappedValue(s, mapper, 2, out interactorStats.attackSpeed);
        Helpers.SetMappedValue(s, mapper, 3, out interactorStats.range);
        Helpers.SetMappedValue(s, mapper, 4, out interactorStats.cooldown);
        Helpers.SetMappedValue(s, mapper, 5, out interactorStats.pierce);
        Helpers.SetMappedValue(s, mapper, 6, out interactorStats.projectiles);
        Helpers.SetMappedValue(s, mapper, 7, out interactorStats.spread);
        Helpers.SetMappedValue(s, mapper, 8, out interactorStats.speedWhileAiming);
        Helpers.SetMappedValue(s, mapper, 9, out interactorStats.criticalChance);
        Helpers.SetMappedValue(s, mapper, 10, out interactorStats.criticalMultiplier);
        Helpers.SetMappedValue(s, mapper, 11, out interactorStats.magazine);
        Helpers.SetMappedValue(s, mapper, 12, out interactorStats.magazineReloadTime);
        Helpers.SetMappedValue(s, mapper, 13, out interactorStats.dps);

        interactorStats.CalculateDPS();

        weapon currentInteractor = (weapon)System.Enum.Parse(typeof(weapon), name);
        DataManager.dictInteractors.Add(currentInteractor, this);
    }

}
