using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum planetSize { small, medium, large }
public enum planetDangerosity { peaceful, agitated, medium, violent, hardcore }
public enum planetResourceScarcity { rare, uncommon, medium, common, abundant }

[System.Serializable]
public class PlanetData
{
    public planetSize size;
    public planetDangerosity dangerosity;
    public planetResourceScarcity violetScarcity;
    public planetResourceScarcity orangeScarcity;
    public planetResourceScarcity blueScarcity;
}
