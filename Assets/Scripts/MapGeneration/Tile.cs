using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;

[CreateAssetMenu(fileName = "Data", menuName = Vault.other.scriptableObjectMenu + "gameTile", order = 1)]
public class Tile : ScriptableObject
{
    public int weight = 1;
    [HideInInspector] private List<GameObject> tileObjects;
    public bool hasLimitedAmount;

    [ConditionalField(nameof(hasLimitedAmount))]
    public int maxAmount;
    public int minAmount;

    [HideInInspector] public List<TileConstraint> constraints;
    [HideInInspector] public int currentAmount = 0;

    public List<Tile> getApplicableConstraints(int distance)    //TODO : remove
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
        constraints = new List<TileConstraint>();
    }

    public void setSpecificAmount(int amount)
    {
        minAmount = amount;
        maxAmount = amount;
    }

    public void setTileObjects(List<GameObject> values)
    {
        tileObjects = values;
    }
    
    public void setTileObjects(GameObject value)
    {
        tileObjects = new List<GameObject> {value};
    }

    public GameObject getTileObject()
    {
        try
        {
            return tileObjects.getRandom();
        }
        catch (Exception e)
        {
            throw new Exception($"Tile {name} has no tile objects defined");
        }
        
    }



}
