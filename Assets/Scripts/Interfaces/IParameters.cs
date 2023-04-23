using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParameters
{
    public static int test;
    public static int maxViolet { get; set; }
    public static int maxOrange { get; private set; }
    public static int maxGreen { get; private set; }

    public static int fillAmountViolet { get; private set; }
    public static int fillAmountOrange { get; private set; }
    public static int fillAmountGreen { get; private set; }

    public static int maxHealth;
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
    //public static float attractorForce { get; private set; }
    float attractorForce { get; set; }
}
