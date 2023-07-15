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
    panelTarget target;

    protected override void Awake()
    {
        base.Awake();

        try
        {
            target = GetComponentInParent<SkillTreePanel>().target;
        }
        catch
        {
            target = panelTarget.none;
        }

        LocalizationManager.LocalizeTextField(upgradeName + Vault.key.ButtonTitle, titleText);
        LocalizationManager.LocalizeTextField(upgradeName + Vault.key.ButtonDescription, descriptionText);

        if (upgradeName.IsNullOrEmpty()) return;
        UpgradeData upgradeData = DataManager.dictUpgrades[upgradeName];
        greenLifeCost = upgradeData.costGreen;
        yellowLifeCost = upgradeData.costOrange;
        effects = upgradeData.effects.Copy();

        foreach (Effect effect in effects)
        {
            effect.target = target;
        }

        greenCostText.text = greenLifeCost.ToString();
        yellowCostText.text = yellowLifeCost.ToString();

        activateButton = upgradeData.upgradesEnabled;
        desactivateButton = upgradeData.upgradesDisabled;
        desactivateButton.TryAdd(upgradeName);
    }

    public void setText(string key)
    {
        titleText.SetText(key);
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
