using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private GameObject ghost;
    [SerializeField] private int spawnsEvery = 1;
    private int spawnCounter = 0;

    public override void Setup(PlayerData stats)
    {
        base.Setup(stats);
        Ennemy.registerListener(this);
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }

    public void onEnnemyDeath(Vector2 position)
    {
        spawnCounter++;
        if (spawnCounter >= spawnsEvery)
        {
            spawnCounter = 0;
            SpawnGhost(position);
        }
    }

    public void SpawnGhost(Vector2 position)
    {
        Instantiate(this.ghost);
        ghost.transform.position = position;
    }
}
