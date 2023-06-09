using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePanelsManager : MonoBehaviour
{
    [Header("Characters")]
    public GameObject pistolero;

    [Header("Weapons")]
    public GameObject gun;

    [Header("Tools")]
    public GameObject tool;

    [Header("Ship")]
    public GameObject ship;

    [Header("Powers")]
    public GameObject minerBot;

    [Header("NextPanel")]
    public GameObject next;

    //will later use a switch to get the panel of the current character 
    public GameObject getCharacterPanel()
    {
        return pistolero;
    }

    public GameObject getWeaponPanel()
    {
        return gun;
    }

    public GameObject getToolPanel()
    {
        return tool;
    }

    public GameObject getShipPanel()
    {
        return ship;
    }

    public GameObject getPower1()
    {
        switch (PlayerManager.power1)
        {
            case power.minerBot:
                return minerBot;

            default:
                return null;
        }
    }

    public GameObject getPower2()
    {
        switch (PlayerManager.power2)
        {
            case power.minerBot:
                return minerBot;

            default:
                return null;
        }
    }

    public GameObject getNext()
    {
        return next;
    }
}
