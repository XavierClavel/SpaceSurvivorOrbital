using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //To delete after the switch to excel files
    [Header("Resources parameters")]
    [SerializeField] private int _maxViolet = 2;
    [SerializeField] private int _maxOrange = 2;
    [SerializeField] private int _maxGreen = 2;

    [SerializeField] private int _fillAmountViolet = 20;
    [SerializeField] private int _fillAmountOrange = 20;
    [SerializeField] private int _fillAmountGreen = 20;


    [Header("Player parameters")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _baseSpeed = 30f;
    [SerializeField] private float _damageResistanceMultiplier = 0f;
    [SerializeField] private float _invulnerabilityFrameDuration = 0.2f;


    [Header("Weapon parameters")]
    [SerializeField] private Vector2Int _baseDamage = new Vector2Int(50, 75);
    [SerializeField] private int _attackSpeed = 10;
    [SerializeField] private float _range = 30;

    [SerializeField] private float _bulletReloadTime = 0.5f;
    [SerializeField] private float _magazineReloadTime = 2f;

    [SerializeField] private float _criticalChance = 0.2f;    //between 0 and 1
    [SerializeField] private float _criticalMultiplier = 2f;  //superior to 1

    [SerializeField] private int _pierce = 0;
    [SerializeField] private float _speed_aimingDemultiplier = 0.7f;
    [SerializeField] private int _magazine = 6;

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


    [Header("Tool parameters")]
    [SerializeField] private int _toolPower = 50;
    [SerializeField] private float _toolReloadTime = 0.5f;
    [SerializeField] private float _toolRange;

    [Header("Attractor parameters")]
    [SerializeField] private float _attractorRange = 2.5f;
    [SerializeField] private float _attractorForce = 2.5f;

    [Header("Others")]
    [SerializeField] Weapon _weapon;
    [SerializeField] Tool _tool;

    [Header("Bonus")]
    [SerializeField] public static bool activateRadar = false;
    [SerializeField] public static bool activateShipArrow = false;


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


    public static Vector2Int baseDamage { get; private set; }
    public static int attackSpeed { get; private set; }
    public static float range { get; private set; }

    public static float bulletReloadTime { get; private set; }
    public static float magazineReloadTime { get; private set; }

    public static float criticalChance { get; private set; }
    public static float criticalMultiplier { get; private set; }

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

    public static PlayerManager instance;

    public static int amountViolet { get; private set; }
    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }

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


            maxHealth = _maxHealth;
            baseSpeed = _baseSpeed;
            damageResistanceMultiplier = _damageResistanceMultiplier;
            invulnerabilityFrameDuration = _invulnerabilityFrameDuration;


            baseDamage = _baseDamage;
            attackSpeed = _attackSpeed;
            range = _range;

            bulletReloadTime = _bulletReloadTime;
            magazineReloadTime = _magazineReloadTime;

            criticalChance = _criticalChance;
            criticalMultiplier = _criticalMultiplier;

            pierce = _pierce;
            speed_aimingDemultiplier = _speed_aimingDemultiplier;
            magazine = _magazine;

            statusEffect = _effect;


            poisonDamage = _poisonDamage;
            poisonDuration = _poisonDuration;
            poisonPeriod = _poisonPeriod;

            fireDamage = _fireDamage;
            fireDuration = _fireDuration;
            firePeriod = _firePeriod;

            iceSpeedMultiplier = _iceSpeedMultiplier;
            iceDuration = _iceDuration;


            toolPower = _toolPower;
            toolReloadTime = _toolReloadTime;
            toolRange = _toolRange;

            attractorRange = _attractorRange;
            attractorForce = _attractorForce;

            weapon = _weapon;
            tool = _tool;
        }
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public static void GatherResourceGreen() => amountGreen++;
    public static void GatherResourceOrange() => amountOrange++;

    public static void GatherResourceViolet() => amountViolet++;

    public static void SetControlMode(bool boolean) => isPlayingWithGamepad = boolean;

    public static void ApplyModification(Effect effect)
    {
        switch (effect.effect)
        {
            case effectType.maxViolet:
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
                baseDamage = effect.ApplyOperation(baseDamage);
                break;

            case effectType.attackSpeed:
                attackSpeed = effect.ApplyOperation(attackSpeed);
                break;

            case effectType.range:
                range = effect.ApplyOperation(range);
                break;

            case effectType.bulletReloadTime:
                bulletReloadTime = effect.ApplyOperation(bulletReloadTime);
                break;

            case effectType.magazineReloadTime:
                magazineReloadTime = effect.ApplyOperation(magazineReloadTime);
                break;

            case effectType.criticalChance:
                criticalChance = effect.ApplyOperation(criticalChance);
                break;

            case effectType.criticalMultiplier:
                criticalMultiplier = effect.ApplyOperation(criticalMultiplier);
                break;

            case effectType.pierce:
                pierce = effect.ApplyOperation(pierce);
                break;

            case effectType.speed_aimingDemultiplier:
                speed_aimingDemultiplier = effect.ApplyOperation(speed_aimingDemultiplier);
                break;

            case effectType.magazine:
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

            case effectType.toolPower:
                toolPower = effect.ApplyOperation(toolPower);
                break;

            case effectType.toolReloadTime:
                toolReloadTime = effect.ApplyOperation(toolReloadTime);
                break;

            case effectType.toolRange:
                toolRange = effect.ApplyOperation(toolRange);
                break;

            case effectType.attractorRange:
                attractorRange = effect.ApplyOperation(attractorRange);
                break;

            case effectType.attractorForce:
                attractorForce = effect.ApplyOperation(attractorForce);
                break;

            case effectType.weapon:
                weapon = effect.ApplyOperation(weapon);
                break;

            case effectType.tool:
                tool = effect.ApplyOperation(tool);
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
}
