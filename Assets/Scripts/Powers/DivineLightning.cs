using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DivineLightning : Power
{
    Vector2 range = new Vector2(14f, 8f);
    LayerMask mask;
    [SerializeField] ParticleSystem lightningStrikePs;
    [SerializeField] Animator animator;

    ComponentPool<ParticleSystem> pool;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
        effect = status.lightning;

        pool = new ComponentPool<ParticleSystem>(lightningStrikePs).setTimer(0.5f);
    }
    //TODO : particle system pool
    protected override void onUse()
    {
        Vector3 hitPoint = playerTransform.position + Helpers.getRandomPositionInRadius(range, shape.square);
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        Hit(collidersInRadius);

        ParticleSystem ps = pool.get(hitPoint);
        

        
        ps.startSize = stats.range * 0.5f;
        ps.Play();

        Vector3 particlePosition = ps.transform.position;
        animator.transform.position = particlePosition;
        animator.enabled = true;

    }
}