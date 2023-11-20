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
    public static float iceDuration;

    public static float lightningDuration;

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
        SetValue(ref ConstantsData.iceSlowdown, "Ice_Slowdown");
        
        SetValue(ref ConstantsData.lightningDuration, "Lightning_Duration");
    }
    
    private void SetValue<T>(ref T variable, string key)
    {
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
