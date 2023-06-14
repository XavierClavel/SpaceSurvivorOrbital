using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameData", menuName = "Space Survivor 2D/GameData", order = 0)]
public class GameData : ScriptableObject
{
    [Header("Ressources Parameters")]
    public int maxViolet;
    public int maxOrange;
    public int maxGreen;

    public int fillAmountViolet;
    public int fillAmountOrange;
    public int fillAmountGreen;

    [Header("Player Parameters")]
    public float invulnerabilityFrameDuration;
    public status effect;

    [Header("Effects Parameters")]
    public int poisonDamage;
    public float poisonDuration;
    public float poisonPeriod;

    public int fireDamage;
    public float fireDuration;
    public float firePeriod;

    public float iceSpeedMultiplier;
    public float iceDuration;

    [Header("Powers")]
    public int minerBotPower = 5;
    public float minerBotSpeed = 1f;


}
