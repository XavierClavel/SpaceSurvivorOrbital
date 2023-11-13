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

    public static int getSize()
    {
        return planetData.size switch
        {
            planetSize.small => 3,
            planetSize.medium => 5,
            planetSize.large => 7,
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
            planetResourceScarcity.rare => 0.1f,
            planetResourceScarcity.medium => 0.2f,
            planetResourceScarcity.common => 0.3f,
            _ => 0.2f
        };
    }

    static int getResourceAmount(planetResourceScarcity scarcity)
    {
        return (int)Mathf.Ceil((getArea() - 2) * getScarcity(scarcity));
    }

    public static int getDensAmount()
    {
        return getResourceAmount(planetData.violetScarcity);
    }

    public static int getOrangeAmount()
    {
        return getResourceAmount(planetData.orangeScarcity);
    }

    public static int getGreenAmount()
    {
        return getResourceAmount(planetData.denScarcity);
    }

    public static planetResourceScarcity getVioletScarcity()
    {
        return planetData.violetScarcity;
    }

    public static planetResourceScarcity getOrangeScarcity()
    {
        return planetData.orangeScarcity;
    }

    public static planetResourceScarcity getGreenScarcity()
    {
        return planetData.denScarcity;
    }

    public static int getDifficulty()
    {
        return planetData.difficulty;
    }

}
