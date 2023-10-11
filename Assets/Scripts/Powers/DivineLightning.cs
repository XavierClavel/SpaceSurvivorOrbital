using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DivineLightning : Power
{
    Vector2 range = new Vector2(14f, 8f);
    LayerMask mask;
    [SerializeField] ParticleSystem lightningStrikePs;

    protected override void Start()
    {
        base.Start();

        Debug.Log("lightning active");

        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
        effect = status.lightning;
    }
    //TODO : particle system pool
    protected override void onUse()
    {
        Debug.Log("hit");
        Vector3 hitPoint = playerTransform.position + Helpers.getRandomPositionInRadius(range, shape.square);
        Debug.Log(playerTransform.position);
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        Hit(collidersInRadius);
        ParticleSystem ps = Instantiate(lightningStrikePs);
        ps.transform.position = hitPoint;
        ps.startSize = stats.range * 0.5f;
        ps.Play();
        Destroy(ps.gameObject, 1f);

    }
}