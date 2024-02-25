using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <p> Projectiles -> How many ennemies to kill before drop </p>
 * <p> Magazine -> How many resources are dropped </p>
 */
public class EnnemiesDropResource : Artefact, IEnnemyListener
{
    private int amount = 0;
    private int dropEvery;
    private int resourcesAmount;

    public override void onSetup()
    {
        Ennemy.registerListener(this);
        dropEvery = stats.projectiles;
        resourcesAmount = stats.magazine;
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }

    public void onEnnemyDeath(Ennemy ennemy)
    {
        amount++;
        if (amount < dropEvery) return;
        amount = 0;
        ObjectManager.SpawnResources(ennemy.transform.position, resourcesAmount);
    }
}
