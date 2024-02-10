using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeButton : TreeButton
{
    int blueCost;
    [SerializeField] TextMeshProUGUI blueCostDisplay;
    [SerializeField] GameObject blueCostObject;

    public override void Initialize(string key)
    {
        base.Initialize(key);

        blueCost = upgradeData.costBlue;
        blueCostDisplay.text = blueCost.ToString();
    }

    protected override bool SpendResources()
    {
        if (PlayerManager.amountBlue < blueCost) return false;

        PlayerManager.SpendUpgradePoints(blueCost);
        return true;
    }

    protected override void HideCost()
    {
        blueCostObject.SetActive(false);
    }

}
