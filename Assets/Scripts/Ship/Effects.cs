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


public enum operationType
{
    add, substract, multiply, divide, assignation
}


public class Effect
{
    public effectType effect;
    public operationType operation;
    public string value;

    public Effect(effectType effect, operationType operation, string value)
    {
        this.effect = effect;
        this.operation = operation;
        this.value = value;
    }


    public virtual void Apply()
    {
        PlayerManager.ApplyModification(this);
    }

    public int ApplyOperation(int parameter)
    {
        int value = Helpers.parseString<int>(this.value);
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

            default:
                throw new System.InvalidOperationException();
        }
        return parameter;
    }

    public float ApplyOperation(float parameter)
    {
        float value = Helpers.parseString<float>(this.value);
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

            default:
                throw new System.InvalidOperationException();
        }
        return parameter;
    }

    public Vector2Int ApplyOperation(Vector2Int parameter)
    {
        Vector2Int value = Helpers.parseString<Vector2Int>(this.value);
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

            default:
                throw new System.InvalidOperationException();
        }

        return parameter;
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

    public Interactor ApplyOperation(Interactor parameter)
    {
        Interactor value = Helpers.parseString<Interactor>(this.value);
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

}
