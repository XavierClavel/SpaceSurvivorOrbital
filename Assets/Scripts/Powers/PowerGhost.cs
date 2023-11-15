using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PowerGhost : Power
{
    public void Awake()
    {
        Ennemy ghost = FindAnyObjectByType<Ennemy>();
        ghost.spawnChance = 1f;
    }
}
