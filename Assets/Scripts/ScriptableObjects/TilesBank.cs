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
    public GameObject green1;
    public GameObject green2;
    public GameObject green3;

    [Header("Orange")]
    public GameObject orange1;
    public GameObject orange2;
    public GameObject orange3;

    public List<Ennemy> ennemies;
}
