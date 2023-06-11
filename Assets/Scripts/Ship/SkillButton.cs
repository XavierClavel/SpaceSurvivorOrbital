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

    [SerializeField] int greenLifeCost;
    [SerializeField] int yellowLifeCost;

    [SerializeField] TextMeshProUGUI greenCostText;
    [SerializeField] TextMeshProUGUI yellowCostText;

    protected override void Awake()
    {
        base.Awake();

        greenCostText.text = greenLifeCost.ToString();
        yellowCostText.text = yellowLifeCost.ToString();
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
}
