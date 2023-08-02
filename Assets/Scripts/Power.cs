using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum powerType { none, creusetout, divineLightning, toxicCloud };

public class Power
{
    powerType type;
    PlayerData data;
    GameObject powerPrefab;

    public Power(powerType type)
    {
        this.type = type;
        //get data from a csv
    }

    public void Instantiate()
    {

    }
}
