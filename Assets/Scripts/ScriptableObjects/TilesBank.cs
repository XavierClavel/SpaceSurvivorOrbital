using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/tilesBank", order = 1)]
public class TilesBank : ScriptableObject
{
    public Color groundColor;
    public GameObject spaceship;
    public List<GameObject> emptyTiles;
    [Header("Violet")]
    public GameObject violet1;
    public GameObject violet2;
    public GameObject violet3;

    [Header("Green")]
    public GameObject greenLow0;
    public GameObject greenLow1;
    public GameObject greenMid0; 
    public GameObject greenMid1;
    public GameObject greenStrong0;
    public GameObject greenStrong1;

    [Header("Yellow")]
    public GameObject yellowLow0;
    public GameObject yellowLow1;
    public GameObject yellowMid0;
    public GameObject yellowMid1;
    public GameObject yellowStrong0;
    public GameObject yellowStrong1;

    [Header("Bonus")]
    public GameObject autel;

    public List<Ennemy> ennemies;
}
