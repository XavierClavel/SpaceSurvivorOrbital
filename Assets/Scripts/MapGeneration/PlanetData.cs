using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum planetSize { small, medium, large }
public enum planetDangerosity { peaceful, medium, hard }
public enum planetResourceScarcity { none, rare, medium, common }
public enum planetType { blue, red, brown }

[System.Serializable]
public class PlanetData
{
    public planetSize size;
    public planetDangerosity dangerosity;
    public planetResourceScarcity violetScarcity;
    public planetResourceScarcity orangeScarcity;
    public planetResourceScarcity greenScarcity;
    public planetType type;
    public bool hasAltar;
}
