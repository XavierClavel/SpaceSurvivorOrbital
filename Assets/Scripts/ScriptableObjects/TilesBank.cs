using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = Vault.other.scriptableObjectMenu + "tilesBank", order = 1)]
public class TilesBank : ScriptableObject
{
    public Color groundColor;
    
    public List<GameObject> emptyTiles;
    
    [Header("Green")]
    public List<GameObject> greenLow;
    public List<GameObject> greenMid;
    public List<GameObject> greenStrong;
    
    [Header("Yellow")]
    public List<GameObject> yellowLow;
    public List<GameObject> yellowMid;
    public List<GameObject> yellowStrong;

    [Header("Extra")]
    public GameObject spaceship;
    public GameObject autel;
    public GameObject den;

    public List<Ennemy> ennemies;
}
