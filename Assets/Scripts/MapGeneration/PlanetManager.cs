using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
            planetSize.small => 5,
            planetSize.medium => 7,
            planetSize.large => 9,
            _ => 1
        };
    }

    public static planetSize getSizeCategory() => planetData.size;

    public static planetType getType()
    {
        return planetData.type;
    }

    private static int getArea()
    {
        return (int)Mathf.Pow(getSize(), 2);
    }

    static float getScarcity(planetResourceScarcity scarcity)
    {
        return scarcity switch
        {
            //planetResourceScarcity.none => 0,
            planetResourceScarcity.rare => ConstantsData.resourceSpawnRateLow,
            planetResourceScarcity.medium => ConstantsData.resourceSpawnRateMid,
            planetResourceScarcity.common => ConstantsData.resourceSpawnRateStrong,
            _ => 0.2f
        };
    }
    

    static int getResourceAmount(planetResourceScarcity scarcity)
    {
        return (int)Mathf.Ceil((getArea() - 1 - getAltarAmount() - getDensAmount()) * getScarcity(scarcity));
    }

    public static int getDensAmount()
    {
        return planetData.size switch
        {
            planetSize.small => 1,
            planetSize.medium => 1,
            planetSize.large => 1,
            _ => 2
        };
    }
    public static int getFountainAmount()
    {
        return planetData.size switch
        {
            planetSize.small => 1,
            planetSize.medium => 1,
            planetSize.large => 1,
            _ => 2
        };
    }

    public static int getChestAmount()
    {
        return planetData.size switch
        {
            planetSize.small => 1,
            planetSize.medium => 1,
            planetSize.large => 1,
            _ => 2
        };
    }

    public static int getAltarAmount()
    {
        return planetData.size switch
        {
            planetSize.small => 1,
            planetSize.medium => 1,
            planetSize.large => 1,
            _ => throw new System.ArgumentOutOfRangeException("Unexpected enum value")
        };
    }

    public static int getRessourceAmount()
    {
        return getResourceAmount(planetData.ressourceScarcity);
    }

    public static bool isBoss()
    {
        return planetData.isBoss;
    }

}
