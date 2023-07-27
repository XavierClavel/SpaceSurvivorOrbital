using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : TreeButton
{
    int upgradePointsCost;
    [SerializeField] TextMeshProUGUI upgradePointsCostDisplay;
    [SerializeField] GameObject upgradePointsCostObject;

    public override void Initialize(string key)
    {
        base.Initialize(key);

        upgradePointsCost = upgradeData.costUpgradePoint;
        upgradePointsCostDisplay.text = upgradePointsCost.ToString();
    }

    protected override bool SpendResources()
    {
        if (PlayerManager.upgradePointsAmount < upgradePointsCost) return false;

        PlayerManager.SpendUpgradePoints(upgradePointsCost);
        return true;
    }

    protected override void HideCost()
    {
        upgradePointsCostObject.SetActive(false);
    }

}
