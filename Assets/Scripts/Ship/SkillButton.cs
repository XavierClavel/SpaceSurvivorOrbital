using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using MyBox;



public enum skillButtonStatus { undefined, locked, unlocked, bought }

public class SkillButton : TreeButton
{

    int greenCost;
    int yellowCost;

    [SerializeField] TextMeshProUGUI greenCostDisplay;
    [SerializeField] TextMeshProUGUI yellowCostDisplay;

    public override void Initialize(string key)
    {
        base.Initialize(key);

        greenCost = upgradeData.costGreen;
        yellowCost = upgradeData.costOrange;

        greenCostDisplay.SetText(greenCost.ToString());
        yellowCostDisplay.SetText(yellowCost.ToString());
    }


    protected override bool SpendResources()
    {
        if (PlayerManager.amountGreen < greenCost || PlayerManager.amountOrange < yellowCost) return false;

        PlayerManager.SpendResources(greenCost, yellowCost);
        return true;
    }

}
