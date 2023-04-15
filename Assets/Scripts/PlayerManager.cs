using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public static float pierce = 0;
    [HideInInspector] public static float damage = 1;
    [HideInInspector] public static float toolPower = 1f;
    [HideInInspector] public static PlayerManager instance;

    [HideInInspector] public static int amountGreen;
    [HideInInspector] public static int amountOrange;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    public static void SaveResources(int nbGreen, int nbOrange)
    {
        amountGreen = nbGreen;
        amountOrange = nbOrange;
    }

    public static bool HasResources(int nbGreen, int nbOrange)
    {
        return nbGreen <= amountGreen && nbOrange <= amountOrange;
    }

    public static void Upgrade(change type, int increase)
    {
        switch (type)
        {
            case change.pierce:
                pierce += increase;
                break;

            case change.damage:
                damage += increase;
                break;

            case change.toolPower:
                toolPower += increase;
                break;
        }
    }
}
