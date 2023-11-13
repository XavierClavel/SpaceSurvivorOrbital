using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum planetSize { small, medium, large }
public enum planetResourceScarcity { none, rare, medium, common }
public enum planetType { ice, mushroom, desert, storm, jungle }

[System.Serializable]
public class PlanetData
{
    public planetSize size;
    public int difficulty;
    public planetResourceScarcity denScarcity;
    public planetResourceScarcity orangeScarcity;
    public planetResourceScarcity greenScarcity;
    public planetType type;
}
