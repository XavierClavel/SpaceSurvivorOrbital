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
        int damage = stats.baseDamage.getRandom();
        bool critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);

        List<Collider2D> colliders = Helpers.OverlapCircularArcAll(player.transform, player.aimVector, stats.range, stats.spread, LayerMask.GetMask(Vault.layer.Resources, Vault.layer.Ennemies));

        foreach (Collider2D collider in colliders)
        {
            ObjectManager.dictObjectToHitable[collider.gameObject].Hit(damage, player.effect, critical);
        }

    }

    protected override void onStartUsing()
    {

    }
    protected override void onStopUsing()
    {

    }
}
