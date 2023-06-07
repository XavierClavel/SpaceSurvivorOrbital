using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TileWaveFunction
{
    //public List<tileType> possibleStates = ((tileType[])Enum.GetValues(typeof(tileType))).ToList();
    public List<Tile> possibleStates = new List<Tile>();
    public int entropy;
    public Vector2Int index;
    public TileWaveFunction(List<Tile> tileList, Vector2Int index)    //constructor
    {
        this.possibleStates = tileList.Copy();
        this.entropy = tileList.Count;
        this.index = index;
    }

    public void ReduceWaveFunction(Tile conflictualTile)
    {
        possibleStates.TryRemove(conflictualTile);
    }

    public void ReduceWaveFunction(List<Tile> conflictualTiles)
    {
        possibleStates.RemoveList(conflictualTiles);
    }

    public Tile CollapseWaveFunction()
    {
        foreach (Tile tile in possibleStates)
        {
            Debug.Log(tile.name);
        }
        Debug.Log("__________________");
        Tile tileToPlace = TileManager.getTileToPlace(possibleStates);

        Debug.Log("result : " + (tileToPlace == null ? "null" : tileToPlace.name));
        if (tileToPlace == null) return getWeightedRandomTile();

        /*
        if (TileManager.tilesInAdvance > 5) return getWeightedRandomTile();
        if (!Helpers.ProbabilisticBool(1f / (float)TileManager.tilesInAdvance))
        {
            return getWeightedRandomTile();
        }
        */

        return tileToPlace;

    }

    Tile getWeightedRandomTile()
    {
        List<Tile> weightedStateList = new List<Tile>();
        foreach (Tile tile in possibleStates)
        {
            for (int i = 0; i < tile.weight; i++)
            {
                weightedStateList.Add(tile);
            }
            //Debug.Log(tile.name);
        }
        //Debug.Log("____________________");
        return weightedStateList.getRandom();
    }

}
