using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlanetManager
{
    static PlanetData planetData;

    public static void setData(PlanetData data)
    {
        planetData = data;
    }

    public static bool hasData()
    {
        return planetData != null;
    }

    /// <summary>
    /// Total length, not radius
    /// </summary>
    /// <returns></returns>
    public static int getSize()
    {
        return planetData.size switch
        {
            planetSize.small => 3,
            planetSize.medium => 5,
            planetSize.large => 9,
            _ => 5
        };
    }

    public static planetType getType()
    {
        return planetData.type;
    }

    public static int getArea()
    {
        return (int)Mathf.Pow(getSize(), 2);
    }

    static float getScarcity(planetResourceScarcity scarcity)
    {
        return scarcity switch
        {
            planetResourceScarcity.none => 0f,
            planetResourceScarcity.rare => ConstantsData.resourceSpawnRateLow,
            planetResourceScarcity.medium => ConstantsData.resourceSpawnRateMid,
            planetResourceScarcity.common => ConstantsData.resourceSpawnRateStrong,
            _ => 0.2f
        };
    }
    
    static float getDenScarcity(planetResourceScarcity scarcity)
    {
        return scarcity switch
        {
            planetResourceScarcity.none => getDenScarcity(planetResourceScarcity.medium),
            planetResourceScarcity.rare => ConstantsData.denSpawnRateLow,
            planetResourceScarcity.medium => ConstantsData.denSpawnRateMid,
            planetResourceScarcity.common => ConstantsData.denSpawnRateStrong,
            _ => 0.2f
        };
    }

    static int getResourceAmount(planetResourceScarcity scarcity)
    {
        return (int)Mathf.Ceil((getArea() - 2) * getScarcity(scarcity));
    }
    
    static int getDensAmount(planetResourceScarcity scarcity)
    {
        return (int)Mathf.Ceil((getArea() - 2) * getDenScarcity(scarcity));
    }

    public static int getDensAmount()
    {
        return getDensAmount(planetData.denScarcity);
    }

    public static int getOrangeAmount()
    {
        return getResourceAmount(planetData.orangeScarcity);
    }

    public static int getGreenAmount()
    {
        return getResourceAmount(planetData.greenScarcity);
    }

    public static planetResourceScarcity getVioletScarcity()
    {
        return planetData.denScarcity;
    }

    public static planetResourceScarcity getOrangeScarcity()
    {
        return planetData.orangeScarcity;
    }

    public static planetResourceScarcity getGreenScarcity()
    {
        return planetData.greenScarcity;
    }

    public static int getDifficulty()
    {
        return planetData.difficulty;
    }

}
