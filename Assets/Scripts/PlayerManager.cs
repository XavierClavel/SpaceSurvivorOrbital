using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum power { none, minerBot };

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameData gameData;

    public static bool activateRadar = false;
    public static bool activateShipArrow = false;
    public static bool activateMinerBotAttractor = false;


    //Static accessors


    public static int maxViolet { get; private set; }
    public static int maxOrange { get; private set; }
    public static int maxGreen { get; private set; }

    public static int fillAmountViolet { get; private set; }
    public static int fillAmountOrange { get; private set; }
    public static int fillAmountGreen { get; private set; }

    public static int maxHealth { get; set; }
    public static float baseSpeed { get; private set; }
    public static float damageResistanceMultiplier { get; private set; }
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


    public static int toolPower { get; private set; }

    public static float toolReloadTime { get; private set; }
    public static float toolRange { get; private set; }
    public static float attractorRange { get; private set; }
    public static float attractorForce { get; private set; }

    public static Interactor weaponPrefab = null;
    public static Interactor toolPrefab = null;

    public static int minerBotPower { get; private set; }
    public static float mineerBotSpeed { get; private set; }

    public static InteractorStats weaponStats;
    public static InteractorStats toolStats;

    public static PlayerManager instance;

    public static int amountViolet { get; private set; }
    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }

    public static power power1 { get; private set; }
    public static power power2 { get; private set; }
    public static int upgradePointsAmount { get; private set; }

    public static void setBase()
    {
        maxHealth = Vault.baseStats.MaxHealth;
        baseSpeed = Vault.baseStats.Speed;
        damageResistanceMultiplier = Vault.baseStats.DamageResistance;
    }


    public static void setWeapon(InteractorStats interactorStats, Interactor interactor)
    {
        weaponStats = interactorStats;
        weaponPrefab = interactor;
    }



    public static void setTool(InteractorStats interactorData, Interactor interactor)
    {
        toolStats = interactorData;
        toolPrefab = interactor;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            maxViolet = gameData.maxViolet;
            maxOrange = gameData.maxOrange;
            maxGreen = gameData.maxGreen;

            fillAmountViolet = gameData.fillAmountViolet;
            fillAmountOrange = gameData.fillAmountOrange;
            fillAmountGreen = gameData.fillAmountGreen;

            invulnerabilityFrameDuration = gameData.invulnerabilityFrameDuration;

            statusEffect = gameData.effect;


            poisonDamage = gameData.poisonDamage;
            poisonDuration = gameData.poisonDuration;
            poisonPeriod = gameData.poisonPeriod;

            fireDamage = gameData.fireDamage;
            fireDuration = gameData.fireDuration;
            firePeriod = gameData.firePeriod;

            iceSpeedMultiplier = gameData.iceSpeedMultiplier;
            iceDuration = gameData.iceDuration;

            minerBotPower = gameData.minerBotPower;
            mineerBotSpeed = gameData.minerBotSpeed;

            attractorForce = 4;
            attractorRange = 5;

            power1 = power.none;
            power2 = power.none;

            upgradePointsAmount = 0;

        }
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public static void GatherResourceGreen() => amountGreen++;
    public static void GatherResourceOrange() => amountOrange++;

    public static void GatherResourceViolet() => amountViolet++;

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

    public static void ApplyModification(Effect effect)
    {
        InteractorStats interactorStats = null;
        if (effect.target == panelTarget.weapon) interactorStats = weaponStats;
        else if (effect.target == panelTarget.tool) interactorStats = toolStats;

        switch (effect.effect)
        {
            case effectType.maxPurple:
                maxViolet = effect.ApplyOperation(maxViolet);
                break;

            case effectType.maxOrange:
                maxOrange = effect.ApplyOperation(maxOrange);
                break;

            case effectType.maxGreen:
                maxGreen = effect.ApplyOperation(maxGreen);
                break;

            case effectType.fillAmountViolet:
                fillAmountViolet = effect.ApplyOperation(fillAmountViolet);
                break;

            case effectType.fillAmountOrange:
                fillAmountOrange = effect.ApplyOperation(fillAmountOrange);
                break;

            case effectType.fillAmountGreen:
                fillAmountGreen = effect.ApplyOperation(fillAmountGreen);
                break;

            case effectType.maxHealth:
                maxHealth = effect.ApplyOperation(maxHealth);
                break;

            case effectType.baseSpeed:
                baseSpeed = effect.ApplyOperation(baseSpeed);
                break;

            case effectType.damageResistanceMultiplier:
                damageResistanceMultiplier = effect.ApplyOperation(damageResistanceMultiplier);
                break;

            case effectType.baseDamage:
                interactorStats.baseDamage = effect.ApplyOperation(interactorStats.baseDamage);
                break;

            case effectType.attackSpeed:
                interactorStats.attackSpeed = effect.ApplyOperation(interactorStats.attackSpeed);
                break;

            case effectType.range:
                interactorStats.range = effect.ApplyOperation(interactorStats.range);
                break;

            case effectType.bulletReloadTime:
                interactorStats.cooldown = effect.ApplyOperation(interactorStats.cooldown);
                break;

            case effectType.magazineReloadTime:
                interactorStats.magazineReloadTime = effect.ApplyOperation(interactorStats.magazineReloadTime);
                break;

            case effectType.criticalChance:
                interactorStats.criticalChance = effect.ApplyOperation(interactorStats.criticalChance);
                break;

            case effectType.criticalMultiplier:
                interactorStats.criticalMultiplier = effect.ApplyOperation(interactorStats.criticalMultiplier);
                break;

            case effectType.projectiles:
                interactorStats.projectiles = effect.ApplyOperation(interactorStats.projectiles);
                break;

            case effectType.spread:
                interactorStats.spread = effect.ApplyOperation(interactorStats.spread);
                break;

            case effectType.pierce:
                interactorStats.pierce = effect.ApplyOperation(interactorStats.pierce);
                break;

            case effectType.aimingSpeed:
                interactorStats.speedWhileAiming = effect.ApplyOperation(interactorStats.speedWhileAiming);
                break;

            case effectType.magazine:
                interactorStats.magazine = effect.ApplyOperation(interactorStats.magazine);
                break;

            case effectType.effect:
                statusEffect = effect.ApplyOperation(statusEffect);
                break;

            case effectType.poisonDamage:
                poisonDamage = effect.ApplyOperation(poisonDamage);
                break;

            case effectType.poisonDuration:
                poisonDuration = effect.ApplyOperation(poisonDuration);
                break;

            case effectType.poisonPeriod:
                poisonPeriod = effect.ApplyOperation(poisonPeriod);
                break;

            case effectType.fireDamage:
                fireDamage = effect.ApplyOperation(fireDamage);
                break;

            case effectType.fireDuration:
                fireDuration = effect.ApplyOperation(fireDuration);
                break;

            case effectType.firePeriod:
                firePeriod = effect.ApplyOperation(firePeriod);
                break;

            case effectType.toolPower:
                toolPower = effect.ApplyOperation(toolPower);
                break;

            case effectType.toolSpeed:
                toolReloadTime = effect.ApplyOperation(toolReloadTime);
                break;

            case effectType.toolRange:
                toolRange = effect.ApplyOperation(toolRange);
                break;

            case effectType.TOOLAttractorRange:
                attractorRange = effect.ApplyOperation(attractorRange);
                break;

            case effectType.TOOLAttractorForce:
                attractorForce = effect.ApplyOperation(attractorForce);
                break;

            /*
            case effectType.tool:
                tool = effect.ApplyOperation(tool);
                break;
                */

            case effectType.POWERMinerBotPower:
                minerBotPower = effect.ApplyOperation(minerBotPower);
                break;

            case effectType.POWERMinerBotSpeed:
                mineerBotSpeed = effect.ApplyOperation(mineerBotSpeed);
                break;
        }
    }

    public static void SpendResources(int costGreen, int costOrange)
    {
        amountGreen -= costGreen;
        amountOrange -= costOrange;
    }

    public static void SpendPurple(int costPurple)
    {
        amountViolet -= costPurple;
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
