using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;


public class DivineLightning : Power
{
    Vector2 range = new Vector2(14f, 8f);
    LayerMask mask;
    [SerializeField] GameObject lightningStrike;

    GameObjectPool pool;
    [SerializeField] private ElectricZone electricZone;

    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
        //effect = status.lightning;

        pool = new GameObjectPool(lightningStrike).setTimer(stats.projectiles);
    }

    void SpawnEletricZone(Vector3 spawnPoint)
    {
        electricZone = Instantiate(electricZone, playerTransform);
        electricZone.transform.position = spawnPoint;
        electricZone.transform.localScale = Vector3.zero;
        //TODO Fade Out and get smaller or just dispawn
        electricZone.Setup(2f);
    }

    protected override void onUse()
    {
        for (int i = 0; i < stats.projectiles; i++)
        {
            Strike();
        }
    }

    private void Strike()
    {
        Vector3 hitPoint = playerTransform.position + Helpers.getRandomPositionInRadius(range, shape.square);
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        status effect = stats.element;
        if (fullStats.generic.boolA)
        {
            if (Helpers.ProbabilisticBool(0.5f)) effect = status.lightning;
        }
        Hit(collidersInRadius, effect: effect);
        
        //SpawnEletricZone(hitPoint);

        GameObject go = pool.get(hitPoint);
    }
}