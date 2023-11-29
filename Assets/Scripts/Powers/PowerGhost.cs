using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private Ghost ghost;

    [SerializeField] private int spawnsEvery = 2;
    private int spawnCounter = 0;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        Ennemy.registerListener(this);
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }

    public void onEnnemyDeath(Vector2 position)
    {
        Debug.Log("on ennemy death");
        spawnCounter++;
        if (spawnCounter < spawnsEvery) return;
        spawnCounter = 0;
        SpawnGhost(position);
    }
    public void SpawnGhost(Vector2 position)
    {
        Debug.Log("spawn ghost");
        Ghost newGhost = Instantiate(ghost);
        newGhost.transform.position = position;
        newGhost.Setup(fullStats);
    }

}
