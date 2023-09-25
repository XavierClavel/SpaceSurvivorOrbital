using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Interactor
{

    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
    }

    protected override void onUse()
    {
        List<Collider2D> colliders = Helpers.OverlapCircularArcAll(player.transform, player.aimVector, stats.range, stats.spread, LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Ennemies));
        Hit(colliders);
    }

    protected override void onStartUsing()
    {

    }
    protected override void onStopUsing()
    {

    }
}
