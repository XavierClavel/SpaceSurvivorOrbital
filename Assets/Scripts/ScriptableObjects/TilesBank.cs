using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = Vault.other.scriptableObjectMenu + "tilesBank", order = 1)]
public class TilesBank : ScriptableObject
{
    public Color groundColor1;
    public Color groundColor2;
    public Color groundColor3;

    public SpriteRenderer groundForm;

    [Header("Empty")]
    public List<GameObject> empty1;
    public List<GameObject> empty2;
    public List<GameObject> empty3;

    [Header("Ressource")]
    public List<GameObject> ressource1;
    public List<GameObject> ressource2;
    public List<GameObject> ressource3;

    [Header("Altar")]
    public GameObject altar1;
    public GameObject altar2;
    public GameObject altar3;

    [Header("Den")]
    public GameObject den1;
    public GameObject den2;
    public GameObject den3;

    [Header("Other")]
    public GameObject spaceship;

    public List<Ennemy> ennemies;
}
