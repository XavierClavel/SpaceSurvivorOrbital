using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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
    [SerializeField] private float _toolRange;

    [Header("Attractor parameters")]
    [SerializeField] private float _attractorRange = 2.5f;
    [SerializeField] private float _attractorForce = 2.5f;


    //Static accessors

    public static int maxViolet { get; private set; }
    public static int maxOrange { get; private set; }
    public static int maxGreen { get; private set; }

    public static int fillAmountViolet { get; private set; }
    public static int fillAmountOrange { get; private set; }
    public static int fillAmountGreen { get; private set; }

    public static int maxHealth { get; private set; }
    public static float baseSpeed { get; private set; }
    public static float damageResistanceMultiplier { get; private set; }


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

    public static status effect { get; private set; }

    public static int poisonDamage { get; private set; }
    public static float poisonDuration { get; private set; }
    public static float poisonPeriod { get; private set; }

    public static int fireDamage { get; private set; }
    public static float fireDuration { get; private set; }
    public static float firePeriod { get; private set; }

    public static float iceSpeedMultiplier { get; private set; }
    public static float iceDuration { get; private set; }


    public static int toolPower { get; private set; }
    public static float toolRange { get; private set; }
    public static float attractorRange { get; private set; }
    public static float attractorForce { get; private set; }






    [HideInInspector] public static PlayerManager instance;

    [HideInInspector] public static int amountGreen;
    [HideInInspector] public static int amountOrange;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            maxViolet = _maxViolet = 2;
            maxOrange = _maxOrange;
            maxGreen = _maxGreen;

            fillAmountViolet = _fillAmountViolet;
            fillAmountOrange = _fillAmountOrange;
            fillAmountGreen = _fillAmountGreen;


            maxHealth = _maxHealth;
            baseSpeed = _baseSpeed;
            damageResistanceMultiplier = _damageResistanceMultiplier;


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

            effect = _effect;


            poisonDamage = _poisonDamage;
            poisonDuration = _poisonDuration;
            poisonPeriod = _poisonPeriod;

            fireDamage = _fireDamage;
            fireDuration = _fireDuration;
            firePeriod = _firePeriod;

            iceSpeedMultiplier = _iceSpeedMultiplier;
            iceDuration = _iceDuration;


            toolPower = _toolPower;
            toolRange = _toolRange;

            attractorRange = _attractorRange;
            attractorForce = _attractorForce;
        }
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("start");
    }

    public static void SaveResources(int nbGreen, int nbOrange)
    {
        amountGreen = nbGreen;
        amountOrange = nbOrange;
    }

    public static bool HasResources(int nbGreen, int nbOrange)
    {
        return nbGreen <= amountGreen && nbOrange <= amountOrange;
    }

    public static void Upgrade(change type, int increase)
    {
        switch (type)
        {
            case change.pierce:
                pierce += increase;
                break;

            case change.toolPower:
                toolPower += increase;
                break;
        }
    }
}
