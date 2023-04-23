using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/gameTile", order = 1)]
public class Tile : ScriptableObject
{
    public int weight;
    public GameObject go;
    public int maxAmount;
}
