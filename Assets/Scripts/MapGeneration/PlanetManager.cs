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

    public static Color getGroundColor()
    {
        switch (planetData.type)
        {
            case planetType.blue:
                return new Color32(92, 106, 147, 255);

            case planetType.red:
                return new Color32(195, 93, 94, 255);

            case planetType.brown:
                return new Color32(164, 93, 61, 255);

            default:
                return new Color32(92, 106, 147, 255); ;
        }

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

    public static int getInitialCost()
    {
        switch (planetData.dangerosity)
        {
            case planetDangerosity.peaceful:
                return Random.Range(5, 15);

            case planetDangerosity.medium:
                return Random.Range(15, 25);

            case planetDangerosity.hard:
                return Random.Range(25, 35);
        }
        return 15;
    }

}
