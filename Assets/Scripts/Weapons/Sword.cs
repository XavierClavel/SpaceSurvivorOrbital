using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : Interactor
{
    [SerializeField] new Collider2D collider;
    Transform pivot;
    protected override void onStartUsing() { }

    protected override void onStopUsing() { }

    protected override void onUse()
    {
        int damage = stats.baseDamage.getRandom();
        bool critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);

        //Physics2D.Cast

        //ObjectManager.dictObjectToBreakable[other.gameObject].Hit(damage, status.none, critical);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {



    }
}
