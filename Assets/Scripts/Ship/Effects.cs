using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System;

public enum effectType
{
    //Ressources
    maxViolet,
    maxOrange,
    maxGreen,

    fillAmountViolet,
    fillAmountOrange,
    fillAmountGreen,

    //Character
    maxHealth,
    baseSpeed,
    damageResistanceMultiplier,

    //Weapon
    baseDamage,
    attackSpeed,
    range,
    magazine,

    bulletReloadTime,
    magazineReloadTime,

    criticalChance,
    criticalMultiplier,

    projectiles,
    spread,
    pierce,
    speed_aimingDemultiplier,


    //Other
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
    add, substract, multiply, divide, assignation
}


[Serializable]
public class Effect
{
    public effectType effect;
    public operationType operation;

    [ConditionalField(nameof(effect), false,
    effectType.maxViolet, effectType.maxOrange, effectType.maxGreen,
    effectType.fillAmountViolet, effectType.fillAmountOrange, effectType.fillAmountGreen,
    effectType.maxHealth, effectType.attackSpeed, effectType.projectiles, effectType.pierce, effectType.magazine,
    effectType.poisonDamage, effectType.fireDamage,
    effectType.toolPower)]
    public int valueInt;


    [ConditionalField(nameof(effect), false,
    effectType.baseSpeed, effectType.damageResistanceMultiplier,
    effectType.range, effectType.spread,
    effectType.bulletReloadTime, effectType.magazineReloadTime,
    effectType.criticalChance, effectType.criticalMultiplier, effectType.speed_aimingDemultiplier,
    effectType.poisonDuration, effectType.poisonPeriod,
    effectType.fireDuration, effectType.firePeriod,
    effectType.iceSpeedMultiplier, effectType.iceDuration,
    effectType.toolPower, effectType.toolReloadTime, effectType.toolRange,
    effectType.attractorRange, effectType.attractorForce)]
    public float valueFloat;


    [ConditionalField(nameof(effect), false, effectType.baseDamage)]
    public Vector2Int valueV2Int;


    [ConditionalField(nameof(effect), false, effectType.effect)]
    public status valueStatus;

    [ConditionalField(nameof(effect), false, effectType.weapon)]
    public Weapon valueWeapon;

    [ConditionalField(nameof(effect), false, effectType.tool)]
    public Tool valueTool;


    public virtual void Apply()
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

            case operationType.substract:
                parameter -= value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.divide:
                parameter /= value;
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
