using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using MyBox;

public enum effectType
{
    maxViolet,
    maxOrange,
    maxGreen,

    fillAmountViolet,
    fillAmountOrange,
    fillAmountGreen,

    maxHealth,
    baseSpeed,
    damageResistanceMultiplier,

    baseDamage,
    attackSpeed,
    range,

    bulletReloadTime,
    magazineReloadTime,

    criticalChance,
    criticalMultiplier,

    pierce,
    speed_aimingDemultiplier,
    magazine,

    effect,

    poisonDamage,
    poisonDuration,
    poisonPeriod,

    fireDamage,
    fireDuration,
    firePeriod,

    iceSpeedMultiplier,
    iceDuration,

    toolPower,
    toolReloadTime,
    toolRange,

    attractorRange,
    attractorForce,

    weapon,
    tool

}

public enum operationType
{
    add, multiply, assignation
}

[Serializable]
public class Effect
{
    public effectType effect;
    public operationType operation;

    [ConditionalField(nameof(effect), false, effectType.maxViolet, effectType.maxOrange, effectType.maxGreen,
    effectType.fillAmountViolet, effectType.fillAmountOrange, effectType.fillAmountGreen,
    effectType.maxHealth, effectType.attackSpeed, effectType.pierce, effectType.magazine, effectType.poisonDamage,
    effectType.fireDamage, effectType.toolPower)]
    public int valueInt;


    [ConditionalField(nameof(effect), false, effectType.baseSpeed, effectType.damageResistanceMultiplier,
    effectType.range, effectType.bulletReloadTime, effectType.magazineReloadTime, effectType.criticalChance,
    effectType.criticalMultiplier, effectType.speed_aimingDemultiplier, effectType.poisonDuration, effectType.poisonPeriod,
    effectType.fireDuration, effectType.firePeriod, effectType.iceSpeedMultiplier, effectType.iceDuration,
    effectType.toolPower, effectType.toolReloadTime, effectType.toolRange, effectType.attractorRange, effectType.attractorForce)]
    public float valueFloat;


    [ConditionalField(nameof(effect), false, effectType.baseDamage)]
    public Vector2Int valueV2Int;


    [ConditionalField(nameof(effect), false, effectType.effect)]
    public status valueStatus;

    [ConditionalField(nameof(effect), false, effectType.weapon)]
    public Weapon valueWeapon;

    [ConditionalField(nameof(effect), false, effectType.tool)]
    public Tool valueTool;


    public void Apply()
    {
        PlayerManager.ApplyModification(this);
    }

    public int ApplyOperation(int parameter)
    {
        int value = valueInt;
        switch (operation)
        {
            case operationType.add:
                parameter += value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.assignation:
                parameter = value;
                break;
        }
        return parameter;
    }

    public float ApplyOperation(float parameter)
    {
        float value = valueFloat;
        switch (operation)
        {
            case operationType.add:
                parameter += value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.assignation:
                parameter = value;
                break;
        }
        return parameter;
    }

    public Vector2Int ApplyOperation(Vector2Int parameter)
    {
        Vector2Int value = valueV2Int;
        switch (operation)
        {
            case operationType.add:
                parameter += value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.assignation:
                parameter = value;
                break;
        }

        return parameter;
    }

    public status ApplyOperation(status parameter)
    {
        status value = valueStatus;
        switch (operation)
        {
            case operationType.assignation:
                parameter = value;
                break;
        }
        return parameter;
    }

    public Weapon ApplyOperation(Weapon parameter)
    {
        Weapon value = valueWeapon;
        switch (operation)
        {
            case operationType.assignation:
                parameter = value;
                break;
        }
        return parameter;
    }

    public Tool ApplyOperation(Tool parameter)
    {
        Tool value = valueTool;
        switch (operation)
        {
            case operationType.assignation:
                parameter = value;
                break;
        }
        return parameter;
    }
}

public enum skillButtonStatus { undefined, locked, unlocked, bought }

public class SkillButton : MonoBehaviour
{
    public static int greenRessource;
    public static int yellowRessource;

    public int greenLifeCost;
    public int yellowLifeCost;

    public List<SkillButton> activateButton = new List<SkillButton>();
    public List<SkillButton> desactivateButton = new List<SkillButton>();
    public List<Effect> effects = new List<Effect>();

    public TextMeshProUGUI greenCostText;
    public TextMeshProUGUI yellowCostText;

    public skillButtonStatus status { get; private set; }
    [HideInInspector] public Button button;
    Image image;

    public bool isFirst;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        greenCostText.text = greenLifeCost.ToString();
        yellowCostText.text = yellowLifeCost.ToString();
    }


    public void OnClick()
    {
        if (status != skillButtonStatus.unlocked) return;

        if (greenRessource < greenLifeCost || yellowRessource < yellowLifeCost)
        {
            return;
        }

        PlayerManager.SpendResources(greenLifeCost, yellowLifeCost);
        ResourcesDisplay.UpdateDisplay();

        greenRessource -= greenLifeCost;
        yellowRessource -= yellowLifeCost;

        SkillTree.UpdateList(activateButton, skillButtonStatus.unlocked);
        SkillTree.UpdateList(desactivateButton, skillButtonStatus.locked);
        SkillTree.UpdateButton(this, skillButtonStatus.bought);

        /*
        button.interactable = false;
        foreach (SkillButton skillButton in activateButton)
        {
            skillButton.button.interactable = true;
        }

        foreach (SkillButton skillButton in desactivateButton)
        {
            skillButton.button.interactable = false;
        }
        */

        foreach (Effect effect in effects)
        {
            effect.Apply();
        }

    }

    public void UpdateStatus(skillButtonStatus status)
    {
        this.status = status;
        switch (status)
        {
            case skillButtonStatus.bought:
                image.color = Color.green;
                break;

            case skillButtonStatus.unlocked:
                image.color = Color.white;
                break;

            case skillButtonStatus.locked:
                image.color = Color.gray;
                break;
        }
    }

    public Radar radar;

    public void ActiveOnClick()
    {
        radar.isActive = true;
    }
}
