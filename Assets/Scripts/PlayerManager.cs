using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum power { none, minerBot };

public class PlayerManager : MonoBehaviour
{
    [SerializeField] CharacterData characterData;
    [SerializeField] WeaponData weaponData;
    [SerializeField] ToolData toolData;

    //To delete after the switch to excel files
    [Header("Resources parameters")]
    [SerializeField] private int _maxViolet = 2;
    [SerializeField] private int _maxOrange = 2;
    [SerializeField] private int _maxGreen = 2;

    [SerializeField] private int _fillAmountViolet = 20;
    [SerializeField] private int _fillAmountOrange = 20;
    [SerializeField] private int _fillAmountGreen = 20;


    [Header("Player parameters")]
    [SerializeField] private float _invulnerabilityFrameDuration = 0.2f;

    [SerializeField] private status _effect = status.none;


    [Header("Game Parameters")]
    [SerializeField] private int _poisonDamage;
    [SerializeField] private float _poisonDuration;
    [SerializeField] private float _poisonPeriod;

    [SerializeField] private int _fireDamage;
    [SerializeField] private float _fireDuration;
    [SerializeField] private float _firePeriod;

    [SerializeField] private float _iceSpeedMultiplier;
    [SerializeField] private float _iceDuration;

    [Header("Powers")]
    [SerializeField] int _minerBotPower = 10;
    [SerializeField] float _minerBotSpeed = 1f;

    [Header("Bonus")]
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

    public static float bulletReloadTime { get; private set; }
    public static float magazineReloadTime { get; private set; }

    public static float criticalChance { get; private set; }
    public static float criticalMultiplier { get; private set; }

    public static int projectiles { get; private set; }
    public static float spread { get; private set; }
    public static int pierce { get; private set; }
    public static float speed_aimingDemultiplier { get; private set; }
    public static int magazine { get; private set; }

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

    public static Weapon weapon { get; private set; }
    public static Tool tool { get; private set; }

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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            maxViolet = _maxViolet;
            maxOrange = _maxOrange;
            maxGreen = _maxGreen;

            fillAmountViolet = _fillAmountViolet;
            fillAmountOrange = _fillAmountOrange;
            fillAmountGreen = _fillAmountGreen;


            maxHealth = characterData.maxHealth;
            baseSpeed = characterData.baseSpeed;
            damageResistanceMultiplier = characterData.damageResistance;
            invulnerabilityFrameDuration = _invulnerabilityFrameDuration;


            baseDamage = weaponData.baseDamage;
            attackSpeed = weaponData.attackSpeed;
            range = weaponData.range;

            bulletReloadTime = weaponData.bulletReloadTime;
            magazineReloadTime = weaponData.magazineReloadTime;

            criticalChance = weaponData.criticalChance;
            criticalMultiplier = weaponData.criticalMultiplier;

            projectiles = weaponData.projectiles;
            spread = weaponData.spread;
            pierce = weaponData.pierce;
            speed_aimingDemultiplier = weaponData.speedWhileAiming;
            magazine = weaponData.magazine;

            statusEffect = _effect;


            poisonDamage = _poisonDamage;
            poisonDuration = _poisonDuration;
            poisonPeriod = _poisonPeriod;

            fireDamage = _fireDamage;
            fireDuration = _fireDuration;
            firePeriod = _firePeriod;

            iceSpeedMultiplier = _iceSpeedMultiplier;
            iceDuration = _iceDuration;


            toolPower = toolData.power;
            toolReloadTime = toolData.reloadTime;
            toolRange = toolData.range;

            attractorRange = toolData.attractorRange;
            attractorForce = toolData.attractorForce;

            weapon = weaponData.weapon;
            tool = toolData.tool;

            minerBotPower = _minerBotPower;
            mineerBotSpeed = _minerBotSpeed;

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
                bulletReloadTime = effect.ApplyOperation(bulletReloadTime);
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
                speed_aimingDemultiplier = effect.ApplyOperation(speed_aimingDemultiplier);
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

            case effectType.weapon:
                weapon = effect.ApplyOperation(weapon);
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
}
