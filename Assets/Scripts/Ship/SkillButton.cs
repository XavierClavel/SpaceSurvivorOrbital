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

    int greenLifeCost;
    int yellowLifeCost;

    [SerializeField] TextMeshProUGUI greenCostText;
    [SerializeField] TextMeshProUGUI yellowCostText;

    protected override void Awake()
    {
        base.Awake();

        UpgradeData upgradeData = CsvParser.dictUpgrades[upgradeName];
        greenLifeCost = upgradeData.costGreen;
        yellowLifeCost = upgradeData.costOrange;
        effects = upgradeData.effects.Copy();

        greenCostText.text = greenLifeCost.ToString();
        yellowCostText.text = yellowLifeCost.ToString();

        activateButton = upgradeData.upgradesEnabled;
        desactivateButton = upgradeData.upgradesDisabled;
        desactivateButton.Add(upgradeName);
    }


    protected override bool SpendResources()
    {
        if (PlayerManager.amountGreen < greenLifeCost || PlayerManager.amountOrange < yellowLifeCost) return false;

        PlayerManager.SpendResources(greenLifeCost, yellowLifeCost);
        return true;
    }

    public void ActiveRadar()
    {
        Execute(PlayerManager.ActivateRadar);
    }

    public void ActiveShipArrow()
    {
        Execute(PlayerManager.ActivateShipArrow);
    }

    public void ActivateMinerBotAttractor()
    {
        Execute(PlayerManager.ActivateMinerBotAttractor);
    }
}
