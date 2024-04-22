using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ConstantsData
{
    public static float resourceSpawnRateLow;
    public static float resourceSpawnRateMid;
    public static float resourceSpawnRateStrong;
    
    public static float denSpawnRateLow;
    public static float denSpawnRateMid;
    public static float denSpawnRateStrong;
    
    public static float laserOverheatThreshold;
    public static float laserHeatingFactor;
    public static float laserCoolingFactor;
    public static float laserOverheatCoolingFactor;

    public static int fireDamage;
    public static int fireDuration;
    public static float fireStep;

    public static float iceSlowdown;
    public static float iceDamageMultiplier;
    public static float iceDuration;

    public static float lightningDuration;

    public static int resourcesFillAmount;

    public static float invulenerabilityFrame;

    public static float audioMinPitch;
    public static float audioMaxPitch;

    public static int timerDuration;
    public static float laserDamageToSpeed;

    public static float audioFootstepInterval;
    
    public static float protectingWaveCooldown;
    public static int protectingWaveDamage;
    public static float protectingWaveRange;
    public static int protectingWaveKnockback;

    public static int chargeBaseCost;
    public static int chargeCostIncrement;

}

public class ConstantsDataBuilder
{
    
    private Dictionary<string, string> dictLineToValue = new Dictionary<string, string>();

    private  void AddValueToDict(List<string> s)
    {
        dictLineToValue[s[0].Trim()] = s[1].Trim();
    }

    private void BuildData()
    {
        SetValue(ref ConstantsData.resourceSpawnRateLow, "ResourcesSpawnRateLow");
        SetValue(ref ConstantsData.resourceSpawnRateMid, "ResourcesSpawnRateMid");
        SetValue(ref ConstantsData.resourceSpawnRateStrong, "ResourcesSpawnRateStrong");
        
        SetValue(ref ConstantsData.denSpawnRateLow, "DensSpawnRateLow");
        SetValue(ref ConstantsData.denSpawnRateMid, "DensSpawnRateMid");
        SetValue(ref ConstantsData.denSpawnRateStrong, "DensSpawnRateStrong");
        
        SetValue(ref ConstantsData.laserCoolingFactor, "LaserCoolingFactor");
        SetValue(ref ConstantsData.laserHeatingFactor, "LaserHeatingFactor");
        SetValue(ref ConstantsData.laserOverheatThreshold, "LaseOverheatThreshold");
        SetValue(ref ConstantsData.laserOverheatCoolingFactor, "LaserOverheatCoolingFactor");
        
        SetValue(ref ConstantsData.fireDamage, "Fire_Damage");
        SetValue(ref ConstantsData.fireDuration, "Fire_Duration");
        SetValue(ref ConstantsData.fireStep, "Fire_Step");
        
        SetValue(ref ConstantsData.iceDuration, "Ice_Duration");
        SetValue(ref ConstantsData.iceDamageMultiplier, "Ice_DamageMultiplier");
        SetValue(ref ConstantsData.iceSlowdown, "Ice_Slowdown");
        
        SetValue(ref ConstantsData.lightningDuration, "Lightning_Duration");
        
        SetValue(ref ConstantsData.invulenerabilityFrame, "InvulnerabilityFrame");
        SetValue(ref ConstantsData.resourcesFillAmount, "Resources_FillAmount");
        
        SetValue(ref ConstantsData.audioMinPitch, "Audio_MinPitch");
        SetValue(ref ConstantsData.audioMaxPitch, "Audio_MaxPitch");
        
        SetValue(ref ConstantsData.timerDuration, "Timer_Duration");
        SetValue(ref ConstantsData.laserDamageToSpeed, "Laser_DamageToSpeed");
        
        SetValue(ref ConstantsData.audioFootstepInterval, "Audio_FootstepInterval");
        
        SetValue(ref ConstantsData.protectingWaveCooldown, "ProtectingWave_Cooldown");
        SetValue(ref ConstantsData.protectingWaveDamage, "ProtectingWave_Damage");
        SetValue(ref ConstantsData.protectingWaveKnockback, "ProtectingWave_Knockback");
        SetValue(ref ConstantsData.protectingWaveRange, "ProtectingWave_Range");

        SetValue(ref ConstantsData.chargeBaseCost, "Charge_BaseCost");
        SetValue(ref ConstantsData.chargeCostIncrement, "Charge_CostIncrement");
    }
    
    private void SetValue<T>(ref T variable, string key)
    {
        if (!dictLineToValue.ContainsKey(key))
        {
            Debug.LogError($"Missing key \"{key}\".");
            return;
        }
        string value = dictLineToValue[key];
        if (string.IsNullOrEmpty(value)) return;
        try
        {
            variable = Helpers.parseString<T>(dictLineToValue[key]);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse value in column \"{key}\".");
        }

    }

    public void loadText(TextAsset csv)
    {
        try
        {
            List<string> stringArray = csv.text.Split('\n').ToList();
            stringArray.RemoveAt(0);

            foreach (string array in stringArray)
            {
                AddValueToDict(array.Split(';').ToList());
            }

            BuildData();

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to read value in table \"ConstantsData\" : {e}");
        }
    }

}
