using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System;

public enum effectType
{
    none,
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
    damageResistance,

    //Weapon
    WEAPONBaseDamage,
    WEAPONAttackSpeed,
    WEAPONRange,
    WEAPONMagazine,

    WEAPONCooldown,
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


public enum operationType
{
    add, substract, multiply, divide, assignation
}


public class Effect
{
    public effectType effect;
    public operationType operation;
    public string parameterString;
    public delegate object getter<T>(T i);
    public delegate void setter<T>(ref T variable, T value);

    public Effect(effectType effect, operationType operation, string parameter)
    {
        this.effect = effect;
        this.operation = operation;
        this.parameterString = parameter;
    }

    public static void setterGeneric<T>(ref T variable, T value)
    {
        variable = value;
    }


    public void Apply()
    {
        switch (effect)
        {
            case effectType.none:
                throw new System.ArgumentException("no effect");

            case effectType.maxHealth:
                ApplyOperation(ref PlayerManager.maxHealth);
                break;

            case effectType.baseSpeed:
                ApplyOperation(ref PlayerManager.baseSpeed);
                break;

            case effectType.damageResistance:
                ApplyOperation(ref PlayerManager.damageResistance);
                break;

            case effectType.WEAPONBaseDamage:
                ApplyOperation(ref PlayerManager.baseDamage);
                break;

            case effectType.WEAPONAttackSpeed:
                ApplyOperation(ref PlayerManager.attackSpeed);
                break;

            case effectType.WEAPONRange:
                ApplyOperation(ref PlayerManager.range);
                break;

            case effectType.WEAPONCooldown:
                ApplyOperation(ref PlayerManager.cooldown);
                break;

            case effectType.WEAPONPierce:
                ApplyOperation(ref PlayerManager.pierce);
                break;

            case effectType.WEAPONProjectiles:
                ApplyOperation(ref PlayerManager.projectiles);
                break;

            case effectType.WEAPONSpread:
                ApplyOperation(ref )
        }
        ApplyOperationInt(ref PlayerManager.amountGreen);



    }

    void ApplyOperation<T>(ref T variable)
    {
        if ()
    }


    public void ApplyOperation(ref int variable)
    {
        int parameter = Helpers.parseString<int>(parameterString);
        int value = (int)(object)variable;
        switch (operation)
        {
            case operationType.add:
                value += parameter;
                break;

            case operationType.substract:
                value -= parameter;
                break;

            case operationType.multiply:
                value *= parameter;
                break;

            case operationType.divide:
                value /= parameter;
                break;

            case operationType.assignation:
                value = parameter;
                break;

            default:
                throw new System.InvalidOperationException();
        }
        variable = value;
    }

    public void ApplyOperation(ref float variable)
    {
        float parameter = Helpers.parseString<float>(parameterString);
        float variableFloat = (float)(object)variable;
        switch (operation)
        {
            case operationType.add:
                variableFloat += parameter;
                break;

            case operationType.multiply:
                variableFloat *= parameter;
                break;

            case operationType.assignation:
                variableFloat = parameter;
                break;

            default:
                throw new System.InvalidOperationException();
        }
    }

    public void ApplyOperation(ref Vector2Int variable)
    {
        Vector2Int parameter = Helpers.parseString<Vector2Int>(parameterString);
        switch (operation)
        {
            case operationType.add:
                variable += parameter;
                break;

            case operationType.multiply:
                variable *= parameter;
                break;

            case operationType.assignation:
                variable = parameter;
                break;

            default:
                throw new System.InvalidOperationException();
        }
    }

    public status ApplyOperation(status parameter)
    {
        status value = status.none;
        switch (operation)
        {
            case operationType.assignation:
                parameter = value;
                break;

            default:
                throw new System.InvalidOperationException();
        }
        return parameter;
    }

    public void ApplyOperation(ref Interactor variable)
    {
        Interactor parameter = Helpers.parseString<Interactor>(parameterString);
        switch (operation)
        {
            case operationType.assignation:
                variable = parameter;
                break;

            default:
                throw new System.InvalidOperationException();
        }
    }

}
