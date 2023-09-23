using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : Interactor
{
    bool hitting = false;
    const float precision = 5f;

    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
    }

    protected override void onStartUsing() { }

    protected override void onStopUsing() { }

    protected override void onUse()
    {
        if (hitting) return;
        StartCoroutine(nameof(Hit));

        //Physics2D.Cast

        //ObjectManager.dictObjectToBreakable[other.gameObject].Hit(damage, status.none, critical);
    }

    IEnumerator Hit() {
        hitting = true;

        int damage = stats.baseDamage.getRandom();
        bool critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);

        List<Collider2D> collidersAlreadyHit = new List<Collider2D>();

        float stepsAmount = stats.spread / precision;
        int intStepsAmount = (int)stepsAmount;
        intStepsAmount += intStepsAmount % 2 == 0 ?  1 : 0;  //odd number
        int halfSteps = (int)((float)(intStepsAmount-1)*0.5f);

        float stepValue = stats.spread / (float)intStepsAmount;
        float timeStepValue = stepValue / stats.attackSpeed; 
        WaitForSeconds waitStep = Helpers.GetWait(timeStepValue);

        for (int i = -halfSteps; i <= halfSteps; i++) {
            RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, player.aimVector, stats.range + i*stepValue, layerMask);
            foreach (RaycastHit2D hit in hits) {
                if (collidersAlreadyHit.Contains(hit.collider)) {
                    continue;
                }
                ObjectManager.dictObjectToHitable[hit.collider.gameObject].Hit(damage, player.effect, critical);
                collidersAlreadyHit.Add(hits.collider);
            }
            yield return waitStep;
        }

        yield return Helpers.getWait(stats.cooldown)
        hitting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {



    }
}
