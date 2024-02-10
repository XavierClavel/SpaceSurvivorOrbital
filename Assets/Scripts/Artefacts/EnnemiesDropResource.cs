using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesDropResource : Artefact, IEnnemyListener
{
    private int amount = 0;
    private int dropEvery;

    public override void onSetup()
    {
        Ennemy.registerListener(this);
        dropEvery = stats.projectiles;
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
        ObjectManager.SpawnResources(ennemy.transform.position, 1);
    }
}
