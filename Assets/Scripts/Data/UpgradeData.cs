using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData : EffectData
{
    public string name;
    public int costGreen;
    public int costOrange;
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

        SetValue(ref name, Vault.key.Name);
        SetValue(ref costGreen, Vault.key.upgrade.CostGreen);
        SetValue(ref costOrange, Vault.key.upgrade.CostOrange);
        SetValue(ref upgradesEnabled, Vault.key.upgrade.UpgradesEnabled);
        SetValue(ref upgradesDisabled, Vault.key.upgrade.UpgradesDisabled);
        SetValue(ref target, Vault.key.upgrade.Target);
        SetValue(ref row, Vault.key.upgrade.Row);

        Debug.Log(target);

        ProcessEffects(columnTitles, s);

        //if (type == )

        if (!DataManager.dictKeyToDictUpgrades.ContainsKey(target)) DataManager.dictKeyToDictUpgrades[target] = new Dictionary<string, UpgradeData>();
        DataManager.dictKeyToDictUpgrades[target][name] = this;
        DataManager.dictUpgrades.Add(name, this);
    }






}
