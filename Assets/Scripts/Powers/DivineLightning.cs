using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

/**
 * <pre>
 * <p> BaseDamage -> Lightning strike damage </p>
 * <p> Cooldown -> Delay between lightning strikes </p>
 * <p> Projectiles -> Amount of lightning strikes </p>
 * <p> BoolA -> Whether ennemies hit get stun half of the time </p>
 * </pre>
 */
public class DivineLightning : Power
{
    Vector2 range = new Vector2(14f, 8f);
    LayerMask mask;
    [SerializeField] Animator lightningStrike;

    ComponentPool<Animator> pool;
    [SerializeField] private ElectricZone electricZonePrefab;
    private static readonly int animStrike = Animator.StringToHash("Strike");

    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
        //effect = status.lightning;

        pool = new ComponentPool<Animator>(lightningStrike).setTimer(stats.projectiles);
    }

    void SpawnEletricZone(Vector3 spawnPoint)
    {
        ElectricZone electricZone = Instantiate(electricZonePrefab);
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
        Debug.Log(hitPoint);
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        status effect = stats.element;
        SoundManager.PlaySfx(transform, key: "Lighning_Strike");
        if (fullStats.generic.boolA)
        {
            if (Helpers.ProbabilisticBool(0.5f)) effect = status.lightning;
        }
        Hit(collidersInRadius, effect: effect);
        
        //SpawnEletricZone(hitPoint);

        Animator anim = pool.get(hitPoint);
        anim.SetTrigger(animStrike);
    }
}