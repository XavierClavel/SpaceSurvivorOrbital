using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum power { none, minerBot };

public class PlayerManager
{

    public static bool activateRadar = false;
    public static bool activateShipArrow = false;
    public static bool activateMinerBotAttractor = false;


    //Static accessors

    public static int fillAmountViolet { get; private set; }
    public static int fillAmountOrange { get; private set; }
    public static int fillAmountGreen { get; private set; }

    public static float invulnerabilityFrameDuration { get; private set; }

    public static status statusEffect { get; private set; }

    public static int poisonDamage { get; private set; }
    public static float poisonDuration { get; private set; }
    public static float poisonPeriod { get; private set; }

    public static int fireDamage { get; private set; }
    public static float fireDuration { get; private set; }
    public static float firePeriod { get; private set; }

    public static float iceSpeedMultiplier { get; private set; }
    public static float iceDuration { get; private set; }

    public static Interactor weaponPrefab = null;
    public static Interactor toolPrefab = null;


    public static PlayerData weaponData;
    public static PlayerData toolData;

    public static int amountPurple { get; private set; }
    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }

    public static power power1 { get; private set; }
    public static power power2 { get; private set; }
    public static int upgradePointsAmount { get; private set; }

    public static PlayerData playerData = new PlayerData();


    public static void setWeapon(PlayerData interactorData, Interactor interactor)
    {
        weaponData = interactorData;
        weaponPrefab = interactor;
    }



    public static void setTool(PlayerData interactorData, Interactor interactor)
    {
        toolData = interactorData;
        toolPrefab = interactor;
    }

    void Reset()
    {
        power1 = power.none;
        power2 = power.none;

        upgradePointsAmount = 0;
        amountPurple = 0;
        amountGreen = 0;
        amountOrange = 0;

        playerData = new PlayerData();
        weaponData = new PlayerData();
        toolData = new PlayerData();

    }

    public static void GatherResourceGreen() => amountGreen++;
    public static void GatherResourceOrange() => amountOrange++;

    public static void GatherResourceViolet() => amountPurple++;

    public static void SetControlMode(bool boolean) => isPlayingWithGamepad = boolean;

    public static void AcquirePower(power newPower)
    {
        if (power1 == power.none) power1 = newPower;
        else if (power2 == power.none) power2 = newPower;
    }

    public static void AcquireUpgradePoint()
    {
        upgradePointsAmount++;
    }

    public static void SpendUpgradePoints(int amount)
    {
        upgradePointsAmount -= amount;
    }

    public static PlayerData getPlayerData(panelTarget target)
    {
        switch (target)
        {
            case panelTarget.character:
                return playerData;

            case panelTarget.weapon:
                return weaponData;

            case panelTarget.tool:
                return toolData;

            case panelTarget.ship:
                return playerData;

            default:
                throw new System.ArgumentException(target.ToString());
        }
    }

    public static void ApplyModification(Effect effect)
    {
        Debug.Log(effect.target);
        if (effect.effect == effectType.unlocks) effect.Unlock();
        else if (effect.effect == effectType.weapon) effect.ApplyOperation(ref weaponPrefab);
        else if (effect.effect == effectType.tool) effect.ApplyOperation(ref toolPrefab);
        else getPlayerData(effect.target).ApplyEffect(effect);
    }

    public static void SpendResources(int costGreen, int costOrange)
    {
        amountGreen -= costGreen;
        amountOrange -= costOrange;
    }

    public static void SpendPurple(int costPurple)
    {
        amountPurple -= costPurple;
    }

    public static void ActivateRadar()
    {
        activateRadar = true;
    }
    public static void ActivateShipArrow()
    {
        activateShipArrow = true;
    }

    public static void ActivateMinerBotAttractor()
    {
        activateMinerBotAttractor = true;
    }

    public static void ResetTimer()
    {
        currentTimer = 0;
    }
}
