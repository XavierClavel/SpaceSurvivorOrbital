using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <p> Projectiles -> How many ennemies to kill before drop </p>
 * <p> Magazine -> How many resources are dropped </p>
 */
public class EnnemiesDropResource : Artefact, IEnemyListener
{
    private int amount = 0;
    private int dropEvery;
    private int resourcesAmount;

    public override void onSetup()
    {
        EventManagers.enemies.registerListener(this);
        dropEvery = stats.projectiles;
        resourcesAmount = stats.magazine;
    }

    private void OnDestroy()
    {
        EventManagers.enemies.unregisterListener(this);
    }

    public void onEnnemyDeath(Ennemy enemy)
    {
        amount++;
        if (amount < dropEvery) return;
        amount = 0;
        ObjectManager.SpawnResources(enemy.transform.position, resourcesAmount);
    }
}
