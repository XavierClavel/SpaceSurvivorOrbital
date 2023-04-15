using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum change { pierce, damage, toolPower }

[Serializable]
public class Upgrade
{
    [Header("Cost")]
    public int costOrange;
    public int costGreen;
    [Header("Gain")]
    public change type;
    public int increase;
}
