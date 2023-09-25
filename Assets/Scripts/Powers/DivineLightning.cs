using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DivineLightning : Power
{
    Vector2 range = new Vector2(6f, 3f);
    LayerMask mask;

    protected override void Start() {
        base.Start();
        
        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
    }

    protected override void onUse() {
        Vector3 hitPoint = Helpers.getRandomPositionInRadius(range, shape.square);
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        Hit(collidersInRadius);
    }
}