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
        collider.enabled = true;
        //ROTATE w/continous collision detection
        //need to separate the instantiated weapon from the player to avoid the player using it as a spinning blade
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int damage = stats.baseDamage.getRandom();
        bool critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);

        ObjectManager.dictObjectToBreakable[other.gameObject].Hit(damage, status.none, critical);
    }
}
