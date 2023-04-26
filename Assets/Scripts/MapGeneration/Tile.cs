using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/gameTile", order = 1)]
public class Tile : ScriptableObject
{
    public int weight = 1;
    public GameObject tileObject;
    public int maxAmount;
    [SerializeField] List<TileConstraint> constraints;

    public List<Tile> getApplicableConstraints(int distance)
    {
        List<Tile> concernedTiles = new List<Tile>();
        foreach (TileConstraint constraint in constraints)
        {
            if (constraint.distance <= distance) concernedTiles.Add(constraint.otherTile);
        }
        return concernedTiles;
    }
}
