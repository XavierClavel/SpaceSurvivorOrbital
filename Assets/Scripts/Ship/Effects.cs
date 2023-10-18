using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System;

public enum effectType
{
    none,
    //Ressources
    maxPurple,
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
    aimingSpeed,


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
    toolSpeed,
    toolRange,

    attractorRange,
    attractorForce,

    weapon,
    tool,

    POWERMinerBotPower,
    POWERMinerBotSpeed,
    unlocks

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
    public string target;

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
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {value}");
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
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {value}");
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

            case operationType.substract:
                parameter -= value;
                break;

            case operationType.multiply:
                parameter *= value;
                break;

            case operationType.divide:
                parameter = new Vector2Int(parameter.x / value.x, parameter.y / value.y);
                break;

            case operationType.assignation:
                parameter = value;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {value}");
        }

        return parameter;
    }

    public void ApplyOperation(ref int parameter)
    {
        parameter = ApplyOperation(parameter);
    }

    public void ApplyOperation(ref float parameter)
    {
        parameter = ApplyOperation(parameter);
    }

    public void ApplyOperation(ref Vector2Int parameter)
    {
        parameter = ApplyOperation(parameter);
    }

    public void Unlock()
    {
        List<string> value = Helpers.ParseList(this.value);
        foreach (string s in value) UnlockItem(s);
    }

    void UnlockItem(string s)
    {
        switch (s)
        {
            case Vault.unlockable.Radar:
                PlayerManager.ActivateRadar();
                break;

            case Vault.unlockable.ShipIndicator:
                PlayerManager.ActivateShipArrow();
                break;

            default:
                throw new System.ArgumentException($"Unlockable \"{s}\" was not found");
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

    public void ApplyOperation(ref Interactor parameter)
    {
        Interactor value = ScriptableObjectManager.dictKeyToWeaponHandler[this.value.Trim()].getWeapon();
        switch (operation)
        {
            case operationType.assignation:
                parameter = value;
                break;

            default:
                throw new System.InvalidOperationException();
        }
        PanelSelector.instance.UpdateButtonSprites();
    }

}
