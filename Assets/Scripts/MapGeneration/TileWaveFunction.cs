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
        if (!possibleStates.Contains(conflictualTile)) return;
        possibleStates.Remove(conflictualTile);
        Debug.Log("possible states remaining " + possibleStates.Count);
    }

    public void ReduceWaveFunction(List<Tile> conflictualTiles)
    {
        possibleStates.RemoveList(conflictualTiles);
        Debug.Log("possible states remaining " + possibleStates.Count);
    }

    public Tile CollapseWaveFunction()
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
