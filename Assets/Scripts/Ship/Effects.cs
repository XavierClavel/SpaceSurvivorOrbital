using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System;

public enum effectType
{
    //Ressources
    CHARACTERMaxViolet,
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
    WEAPONBaseDamage,
    WEAPONAttackSpeed,
    WEAPONRange,
    WEAPONMagazine,

    WEAPONBulletReloadTime,
    WEAPONMagazineReloadTime,

    WEAPONCriticalChance,
    WEAPONCriticalMultiplier,

    WEAPONProjectiles,
    WEAPONSpread,
    WEAPONPierce,
    WEAPONSpeedWhileAimingDecrease,


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

    TOOLPower,
    TOOLReloadTime,
    TOOLRange,

    TOOLAttractorRange,
    TOOLAttractorForce,

    weapon,
    tool,

    POWERMinerBotPower,
    POWERMinerBotSpeed

}

public enum weaponParameter
{
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
}

public enum operationType
{
    add, substract, multiply, divide, assignation
}


[Serializable]
public class WeaponEffect : Effect
{
    public weaponParameter parameter;
    [ConditionalField(nameof(parameter), false,
    weaponParameter.attackSpeed, weaponParameter.projectiles, weaponParameter.pierce, weaponParameter.magazine
    )]
    new public int valueInt;
    new public float valueFloat;
}


[Serializable]
public class Effect
{
    public effectType effect;
    public operationType operation;

    [ConditionalField(nameof(effect), false,
    effectType.CHARACTERMaxViolet, effectType.maxOrange, effectType.maxGreen,
    effectType.fillAmountViolet, effectType.fillAmountOrange, effectType.fillAmountGreen,
    effectType.maxHealth, effectType.WEAPONAttackSpeed, effectType.WEAPONProjectiles, effectType.WEAPONPierce, effectType.WEAPONMagazine,
    effectType.poisonDamage, effectType.fireDamage,
    effectType.TOOLPower,
    effectType.POWERMinerBotPower)]
    public int valueInt;


    [ConditionalField(nameof(effect), false,
    effectType.baseSpeed, effectType.damageResistanceMultiplier,
    effectType.WEAPONRange, effectType.WEAPONSpread,
    effectType.WEAPONBulletReloadTime, effectType.WEAPONMagazineReloadTime,
    effectType.WEAPONCriticalChance, effectType.WEAPONCriticalMultiplier, effectType.WEAPONSpeedWhileAimingDecrease,
    effectType.poisonDuration, effectType.poisonPeriod,
    effectType.fireDuration, effectType.firePeriod,
    effectType.iceSpeedMultiplier, effectType.iceDuration,
    effectType.TOOLPower, effectType.TOOLReloadTime, effectType.TOOLRange,
    effectType.TOOLAttractorRange, effectType.TOOLAttractorForce,
    effectType.POWERMinerBotSpeed)]
    public float valueFloat;


    public bool valueBool;


    [ConditionalField(nameof(effect), false, effectType.WEAPONBaseDamage)]
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
