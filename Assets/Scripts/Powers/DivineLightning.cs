using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DivineLightning : Power
{
    Vector2 range = new Vector2(6f, 3f);
    LayerMask mask;

    protected override void Start()
    {
        base.Start();

        Debug.Log("lightning active");

        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
        effect = status.lightning;
    }

    protected override void onUse()
    {
        Debug.Log("hit");
        Vector3 hitPoint = playerTransform.position + Helpers.getRandomPositionInRadius(range, shape.square);
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        Hit(collidersInRadius);
        Debug.Log(collidersInRadius.Length);
    }
}