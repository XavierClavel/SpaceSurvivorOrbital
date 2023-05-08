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
        this.possibleStates = tileList.ToArray().ToList();
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
        Tile tileToPlace = TileManager.getTileToPlace(possibleStates);
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
        }
        return weightedStateList.getRandom();
    }

}
