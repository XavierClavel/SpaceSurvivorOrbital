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


    public static int maxViolet;
    public static int maxOrange;
    public static int maxGreen;

    public static int fillAmountViolet;
    public static int fillAmountOrange;
    public static int fillAmountGreen;

    public static int maxHealth;
    public static float baseSpeed;
    public static float damageResistance;
    public static float invulnerabilityFrameDuration;

    //Weapon
    public static Vector2Int baseDamage;
    public static int attackSpeed;
    public static float range;

    public static float cooldown;
    public static float magazineReloadTime;

    public static float criticalChance;
    public static float criticalMultiplier;

    public static int projectiles;
    public static float spread;
    public static int pierce;
    public static float speedWhileAiming;
    public static int magazine;

    public static int dps;

    public static status statusEffect;

    public static int poisonDamage;
    public static float poisonDuration;
    public static float poisonPeriod;

    public static int fireDamage;
    public static float fireDuration;
    public static float firePeriod;

    public static float iceSpeedMultiplier;
    public static float iceDuration;


    public static int toolPower;

    public static float toolReloadTime;
    public static float toolRange;
    public static float attractorRange;
    public static float attractorForce;

    public static Interactor weapon;
    public static Tool tool;

    public static int minerBotPower;
    public static float mineerBotSpeed;



    public static PlayerManager instance;

    public static int amountViolet;
    public static int amountGreen;
    public static int amountOrange;

    public static bool isPlayingWithGamepad;
    public static int currentTimer { get; set; }

    public static power power1;
    public static power power2;
    public static int upgradePointsAmount;

    public static void setCharacter(CharacterData characterData)
    {
        maxHealth = characterData.maxHealth;
        baseSpeed = characterData.baseSpeed;
        damageResistance = characterData.damageResistance;
    }

    public static void setInteractor(InteractorData interactorData, Interactor interactor)
    {
        baseDamage = interactorData.baseDamage;
        attackSpeed = interactorData.attackSpeed;
        range = interactorData.range;

        cooldown = interactorData.cooldown;
        magazineReloadTime = interactorData.magazineReloadTime;

        criticalChance = interactorData.criticalChance;
        criticalMultiplier = interactorData.criticalMultiplier;

        projectiles = interactorData.projectiles;
        spread = interactorData.spread;
        pierce = interactorData.pierce;
        speedWhileAiming = interactorData.speedWhileAiming;
        magazine = interactorData.magazine;
        dps = interactorData.dps;

        weapon = interactor;
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
        switch (effect.effect)
        {
            case effectType.CHARACTERMaxViolet:
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

            case effectType.damageResistance:
                damageResistance = effect.ApplyOperation(damageResistance);
                break;

            case effectType.WEAPONBaseDamage:
                baseDamage = effect.ApplyOperation(baseDamage);
                break;

            case effectType.WEAPONAttackSpeed:
                attackSpeed = effect.ApplyOperation(attackSpeed);
                break;

            case effectType.WEAPONRange:
                range = effect.ApplyOperation(range);
                break;

            case effectType.WEAPONCooldown:
                cooldown = effect.ApplyOperation(cooldown);
                break;

            case effectType.WEAPONMagazineReloadTime:
                magazineReloadTime = effect.ApplyOperation(magazineReloadTime);
                break;

            case effectType.WEAPONCriticalChance:
                criticalChance = effect.ApplyOperation(criticalChance);
                break;

            case effectType.WEAPONCriticalMultiplier:
                criticalMultiplier = effect.ApplyOperation(criticalMultiplier);
                break;

            case effectType.WEAPONProjectiles:
                projectiles = effect.ApplyOperation(projectiles);
                break;

            case effectType.WEAPONSpread:
                spread = effect.ApplyOperation(spread);
                break;

            case effectType.WEAPONPierce:
                pierce = effect.ApplyOperation(pierce);
                break;

            case effectType.WEAPONSpeedWhileAimingDecrease:
                speedWhileAiming = effect.ApplyOperation(speedWhileAiming);
                break;

            case effectType.WEAPONMagazine:
                magazine = effect.ApplyOperation(magazine);
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

            case effectType.TOOLPower:
                toolPower = effect.ApplyOperation(toolPower);
                break;

            case effectType.TOOLReloadTime:
                toolReloadTime = effect.ApplyOperation(toolReloadTime);
                break;

            case effectType.TOOLRange:
                toolRange = effect.ApplyOperation(toolRange);
                break;

            case effectType.TOOLAttractorRange:
                attractorRange = effect.ApplyOperation(attractorRange);
                break;

            case effectType.TOOLAttractorForce:
                attractorForce = effect.ApplyOperation(attractorForce);
                break;

            case effectType.tool:
                tool = effect.ApplyOperation(tool);
                break;

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
