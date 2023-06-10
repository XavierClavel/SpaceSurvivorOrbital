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

    public static bool hasAltar()
    {
        return planetData.hasAltar;
    }

    public static int getSize()
    {
        switch (planetData.size)
        {
            case planetSize.small:
                return 5;

            case planetSize.medium:
                return 9;

            case planetSize.large:
                return 17;

            default:
                return 9;
        }
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
        switch (scarcity)
        {
            case planetResourceScarcity.none:
                return 0f;

            case planetResourceScarcity.rare:
                return 0.1f;

            case planetResourceScarcity.medium:
                return 0.2f;

            case planetResourceScarcity.common:
                return 0.3f;
        }
        return 0.2f;
    }

    static int getResourceAmount(planetResourceScarcity scarcity)
    {
        return (int)Mathf.Ceil(getArea() * getScarcity(scarcity));
    }

    public static int getVioletAmount()
    {
        return getResourceAmount(planetData.violetScarcity);
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
        return planetData.violetScarcity;
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
