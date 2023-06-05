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
        Debug.Log(planetData.size);
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

}
