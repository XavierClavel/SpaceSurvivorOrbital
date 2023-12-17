using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <pre>
 * <p> BaseDamage -> Toxic Zone Dps </p>
 * <p> Cooldown -> Delay between toxic zones </p>
 * <p> Projectiles -> Amount of toxic zones </p>
 * <p> BoolA -> Increase player speed </p>
 * <p> BoolB -> Increase player damage </p>
 * <p> BoolC -> Decrease ennemy speed </p>
 * <p> Projectiles -> Amount of toxic zones </p>
 * <p> Projectiles -> Amount of toxic zones </p>
 * </pre>
 */
public class PowerToxicZone : Power
{
    Vector2 range = new Vector2(14f, 8f);

    ComponentPool<ToxicZone> pool;
    [SerializeField] private ToxicZone toxicZonePrefab;
    private static PowerToxicZone instance;
    
    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        instance = this;

        pool = new ComponentPool<ToxicZone>(toxicZonePrefab);
    }
    
    protected override void onUse()
    {
        StartCoroutine(nameof(SpawnToxicZones));
    }

    IEnumerator SpawnToxicZones()
    {
        for (int i = 0; i < stats.projectiles; i++)
        {
            Strike();
            yield return Helpers.GetWait(0.2f);
        }
    }

    private void Strike()
    {
        Vector3 spawnPoint = playerTransform.position + Helpers.getRandomPositionInRadius(range, shape.square);
        ToxicZone toxicZone = pool.get(spawnPoint);
        toxicZone.Setup(stats.baseDamage.x, stats.range);
        //toxicZone.transform.localScale = Vector3.zero;
    }

    public static void recall(ToxicZone toxicZone)
    {
        instance.pool.recall(toxicZone);
    }
}
