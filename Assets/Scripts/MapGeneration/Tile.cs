using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/gameTile", order = 1)]
public class Tile : ScriptableObject
{
    public int weight = 1;
    public GameObject tileObject;
    public bool hasLimitedAmount;

    [ConditionalField(nameof(hasLimitedAmount))]
    public int maxAmount;

    public List<TileConstraint> constraints;
    [HideInInspector] public int currentAmount = 0;

    public List<Tile> getApplicableConstraints(int distance)
    {
        List<Tile> concernedTiles = new List<Tile>();
        foreach (TileConstraint constraint in constraints)
        {
            if (constraint.distance <= distance) concernedTiles.Add(constraint.otherTile);
        }
        return concernedTiles;
    }

    public void Reset()
    {
        currentAmount = 0;
    }



}
