using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData : EffectData
{
    public string key;
    public int costGreen;
    public int costOrange;
    public int costUpgradePoint;
    public List<string> upgradesEnabled = new List<string>();
    public List<string> upgradesDisabled = new List<string>();
    public string target;
    public int row;



    static List<string> columnTitles = new List<string>();

    public static void Initialize(List<string> s)
    {
        columnTitles = InitializeColumnTitles(s);
    }

    public UpgradeData(List<string> s)
    {
        if (s == null || s.Count != columnTitles.Count) return;
        SetDictionary(columnTitles, s);

        SetValue(ref key, Vault.key.Key);
        SetValue(ref upgradesEnabled, Vault.key.upgrade.UpgradesEnabled);
        SetValue(ref upgradesDisabled, Vault.key.upgrade.UpgradesDisabled);
        SetValue(ref target, Vault.key.upgrade.Target);
        SetValue(ref row, Vault.key.upgrade.Row);

        TrySetValue(ref costGreen, Vault.key.upgrade.CostGreen);
        TrySetValue(ref costOrange, Vault.key.upgrade.CostOrange);
        TrySetValue(ref costUpgradePoint, Vault.key.upgrade.CostUpgradePoint);



        ProcessEffects(columnTitles, s);

        panelTarget pTarget = getTarget();

        foreach (Effect effect in effects)
        {
            effect.target = pTarget;
        }

        if (!DataManager.dictKeyToDictUpgrades.ContainsKey(target)) DataManager.dictKeyToDictUpgrades[target] = new Dictionary<string, UpgradeData>();

        DataManager.dictKeyToDictUpgrades[target][key] = this;
        DataManager.dictUpgrades.Add(key, this);
    }

    panelTarget getTarget()
    {
        switch (target)
        {
            case Vault.key.target.Gun:
                return panelTarget.weapon;

            case Vault.key.target.Laser:
                return panelTarget.weapon;

            case Vault.key.target.Pickaxe:
                return panelTarget.tool;

            case Vault.key.target.Pistolero:
                return panelTarget.character;

            case Vault.key.target.Ship:
                return panelTarget.ship;

            default:
                throw new System.ArgumentException($"Unknown target : {target}");
        }
    }






}
