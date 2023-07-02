using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData : PlayerData
{

    public int costGreen;
    public int costOrange;
    public List<string> upgradesEnabled;
    public List<string> upgradesDisabled;

    static List<string> firstLineOverride = new List<string> {
        "CostGreen",
        "CostOrange",
        "UpgradesEnabled",
        "UpgradesDisabled",
    };

    public UpgradeData(List<string> s)
    {
        setEffects(s);

        SetOverrideValue(s, mapper, 0, out costGreen);
        SetOverrideValue(s, mapper, 1, out costOrange);
        SetOverrideValue(s, mapper, 2, out upgradesEnabled);
        SetOverrideValue(s, mapper, 3, out upgradesDisabled);


        DataManager.dictUpgrades.Add(name, this);
    }

    public static void Initialize(List<string> s)
    {
        OverrideInitialize(s, firstLineOverride);
        Debug.Log("initializing ok");
    }


}