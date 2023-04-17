using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum tileType { empty }

[Serializable]
public class Tile
{
    public Vector2Int position;
    GameObject tile;

    public Tile(Vector2Int position, GameObject tile)
    {
        this.position = position;
        this.tile = tile;
    }

}
