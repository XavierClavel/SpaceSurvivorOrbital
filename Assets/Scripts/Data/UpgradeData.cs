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
    public string spriteKey = "";

    static Dictionary<string, panelTarget> dictTargetToPanelTarget = new Dictionary<string, panelTarget> {
        {Vault.key.target.Pistolero, panelTarget.character},

        {Vault.key.target.Gun, panelTarget.weapon},
        {Vault.key.target.Laser, panelTarget.weapon},

        {Vault.key.target.Pickaxe, panelTarget.tool},

        {Vault.key.target.Ship, panelTarget.ship}
    };



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
        if (key == "") return;
        SetValue(ref upgradesEnabled, Vault.key.upgrade.UpgradesEnabled);
        SetValue(ref upgradesDisabled, Vault.key.upgrade.UpgradesDisabled);
        SetValue(ref target, Vault.key.upgrade.Target);
        SetValue(ref row, Vault.key.upgrade.Row);
        SetValue(ref spriteKey, Vault.key.upgrade.SpriteKey);

        TrySetValue(ref costGreen, Vault.key.upgrade.CostGreen);
        TrySetValue(ref costOrange, Vault.key.upgrade.CostOrange);
        TrySetValue(ref costUpgradePoint, Vault.key.upgrade.CostUpgradePoint);



        ProcessEffects(columnTitles, s);

        panelTarget pTarget = dictTargetToPanelTarget[target];

        foreach (Effect effect in effects)
        {
            effect.target = pTarget;
        }

        if (!DataManager.dictKeyToDictUpgrades.ContainsKey(target)) DataManager.dictKeyToDictUpgrades[target] = new Dictionary<string, UpgradeData>();

        DataManager.dictKeyToDictUpgrades[target][key] = this;
        DataManager.dictUpgrades.Add(key, this);
    }






}
