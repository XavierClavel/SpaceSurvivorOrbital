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

    //Weapon
    public static Vector2Int baseDamage { get; private set; }
    public static int attackSpeed { get; private set; }
    public static float range { get; private set; }

    public static float cooldown { get; private set; }
    public static float magazineReloadTime { get; private set; }

    public static float criticalChance { get; private set; }
    public static float criticalMultiplier { get; private set; }

    public static int projectiles { get; private set; }
    public static float spread { get; private set; }
    public static int pierce { get; private set; }
    public static float speedWhileAiming { get; private set; }
    public static int magazine { get; private set; }

    public static int dps { get; private set; }

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

    public static Interactor weapon { get; private set; }
    public static Interactor tool { get; private set; }

    public static int minerBotPower { get; private set; }
    public static float mineerBotSpeed { get; private set; }



    public static PlayerManager instance;

    public static int amountViolet { get; private set; }
    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }

    public static power power1 { get; private set; }
    public static power power2 { get; private set; }
    public static int upgradePointsAmount { get; private set; }

    public static void setCharacter(CharacterData characterData)
    {
        maxHealth = characterData.maxHealth;
        baseSpeed = characterData.baseSpeed;
        damageResistanceMultiplier = characterData.damageResistance;
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

            case effectType.damageResistanceMultiplier:
                damageResistanceMultiplier = effect.ApplyOperation(damageResistanceMultiplier);
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

            case effectType.WEAPONBulletReloadTime:
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
