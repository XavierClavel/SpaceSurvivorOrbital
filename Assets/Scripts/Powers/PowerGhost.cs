using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PowerGhost : Power
{
    public float spawnChance = 1f;

    public void Awake()
    {
        Ennemy ghost = FindAnyObjectByType<Ennemy>();
        ghost.GhostAppear(spawnChance);
    }
}
