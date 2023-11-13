using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileConstraint
{
    public Tile otherTile;
    public int distance;

    public TileConstraint(Tile otherTile, int distance)
    {
        this.otherTile = otherTile;
        this.distance = distance;
    }
}

[System.Serializable]
public class DistanceConstraint
{
    public Tile tile1;
    public Tile tile2;
    public int distance;

    public void Apply()
    {
        //Debug.Log(tile1.name + " and " + tile2.name + " : " + distance);
        tile1.constraints.Add(new TileConstraint(tile2, distance));
        tile2.constraints.Add(new TileConstraint(tile1, distance));
    }
}

[System.Serializable]
public class DistanceConstraintGroup
{
    public List<Tile> tiles = new List<Tile>();
    public int distance;

    public void Apply()
    {
        /*
        foreach (Tile tile in tiles)
        {
            List<Tile> otherTiles = tiles.Copy();
            otherTiles.Remove(tile);
            foreach (Tile otherTile in otherTiles)
            {
                tile.constraints.Add(new TileConstraint(otherTile, distance));
            }
        }
        */
    }
}

[System.Serializable]
public class SelfDistanceConstraint
{
    public Tile tile;
    public int distance;

    public void Apply()
    {
        //tile.constraints.Add(new TileConstraint(tile, distance));
    }
}