using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData
{
    public string key;
    public int costGreen;
    public int costOrange;
    public int costBlue;
    public List<string> upgradesEnabled = new List<string>();
    public List<string> upgradesDisabled = new List<string>();
    public string target;
    public int row;
    public string spriteKey = "";
    public List<Effect> effects = new List<Effect>();

    public bool valueA = false;
    public bool valueB = false;
    public bool valueC = false;

    public void Apply()
    {
        foreach (Effect effect in effects) effect.Apply();
    }

}

public class UpgradeDataBuilder : DataBuilder<UpgradeData>
{

    protected override UpgradeData BuildData(List<string> s)
    {
        UpgradeData value = new UpgradeData();

        SetValue(ref value.key, Vault.key.Key);

        SetValue(ref value.upgradesEnabled, Vault.key.upgrade.UpgradesEnabled);
        SetValue(ref value.upgradesDisabled, Vault.key.upgrade.UpgradesDisabled);
        SetValue(ref value.target, Vault.key.upgrade.Target);
        SetValue(ref value.row, Vault.key.upgrade.Row);
        SetValue(ref value.spriteKey, Vault.key.upgrade.SpriteKey);

        TrySetValue(ref value.costGreen, Vault.key.upgrade.CostGreen);
        TrySetValue(ref value.costOrange, Vault.key.upgrade.CostOrange);
        TrySetValue(ref value.costBlue, Vault.key.upgrade.CostBlue);
        
        TrySetValue(ref value.valueA, "ValueA");
        TrySetValue(ref value.valueB, "ValueB");
        TrySetValue(ref value.valueC, "ValueC");


        ProcessEffects(columnTitles, s, ref value.effects);

        foreach (Effect effect in value.effects)
        {
            effect.target = value.target;
        }

        if (!DataManager.dictKeyToDictUpgrades.ContainsKey(value.target)) DataManager.dictKeyToDictUpgrades[value.target] = new Dictionary<string, UpgradeData>();

        DataManager.dictKeyToDictUpgrades[value.target][value.key] = value;

        return value;
    }

}
