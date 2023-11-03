using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGhost : Power
{

    public void Awake()
    {
        Ennemy ghost = FindAnyObjectByType<Ennemy>();
        ghost.GhostAppear(true);
    }
}
