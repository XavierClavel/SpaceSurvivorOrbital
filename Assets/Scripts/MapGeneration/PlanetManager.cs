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
        return planetData.groundColor;
    }

}
