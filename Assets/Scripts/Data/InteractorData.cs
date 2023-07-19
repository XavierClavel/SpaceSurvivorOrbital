using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorData : EffectData
{
    public string name;
    public PlayerData interactorData = new PlayerData();


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
        SetValue(ref interactorData.interactor.baseDamage, Vault.key.upgrade.BaseDamage);
        SetValue(ref interactorData.interactor.attackSpeed, Vault.key.upgrade.AttackSpeed);
        SetValue(ref interactorData.interactor.range, Vault.key.upgrade.Range);
        SetValue(ref interactorData.interactor.cooldown, Vault.key.upgrade.Cooldown);
        SetValue(ref interactorData.interactor.pierce, Vault.key.upgrade.Pierce);
        SetValue(ref interactorData.interactor.projectiles, Vault.key.upgrade.Projectiles);
        SetValue(ref interactorData.interactor.spread, Vault.key.upgrade.Spread);
        SetValue(ref interactorData.interactor.speedWhileAiming, Vault.key.upgrade.AimingSpeed);
        SetValue(ref interactorData.interactor.criticalChance, Vault.key.upgrade.CriticalChance);
        SetValue(ref interactorData.interactor.criticalMultiplier, Vault.key.upgrade.CriticalChance);
        SetValue(ref interactorData.interactor.magazine, Vault.key.upgrade.Magazine);
        SetValue(ref interactorData.interactor.magazineReloadTime, Vault.key.upgrade.MagazineCooldown);
        SetValue(ref interactorData.interactor.dps, "DPS");

        interactorData.interactor.CalculateDPS();

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
