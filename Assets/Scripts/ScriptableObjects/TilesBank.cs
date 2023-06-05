using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/tilesBank", order = 1)]
public class TilesBank : ScriptableObject
{
    public List<Tile> emptyTiles;
    public List<Tile> violetTiles;
    public List<Tile> orangeTiles;
    public List<Tile> greenTiles;
}
