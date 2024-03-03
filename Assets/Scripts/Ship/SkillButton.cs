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

    [SerializeField] GameObject greenCostObject;
    [SerializeField] GameObject yellowCostObject;


    public override void Initialize(Node node)
    {
        base.Initialize(node);

        greenCost = upgradeData.costGreen;
        yellowCost = upgradeData.costOrange;

        greenCostDisplay.SetText(greenCost.ToString());
        yellowCostDisplay.SetText(yellowCost.ToString());

        if (greenCost == 0) greenCostObject.SetActive(false);
        if (yellowCost == 0) yellowCostObject.SetActive(false);



        //SpriteState spriteState = new SpriteState();
        //spriteState.selectedSprite = buttonSprite.selected;

        //button.spriteState;

    }

    protected override void HideCost()
    {
        greenCostObject.SetActive(false);
        yellowCostObject.SetActive(false);
    }


    protected override bool SpendResources()
    {
        if (PlayerManager.amountGreen < greenCost || PlayerManager.amountOrange < yellowCost) return false;

        PlayerManager.SpendResources(greenCost, yellowCost);
        return true;
    }

}
