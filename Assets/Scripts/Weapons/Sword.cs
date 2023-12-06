using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : Interactor
{
    bool hitting = false;
    const float precision = 5f;
    [SerializeField] private Collider2D swordCollider;
    [SerializeField] private Blade blade;
    private float attackLength;

    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        swordCollider.enabled = false;
        attackLength = 1f / stats.attackSpeed;
    }

    protected override void onStartUsing() { }

    protected override void onStopUsing() { }

    protected override void onUse()
    {
        player.OverrideWeaponRotation();
        SoundManager.PlaySfx(transform, key: "Sword_Slash");
        swordCollider.enabled = true;
        Vector3 eulerDelta = stats.spread * 0.5f * Vector3.forward;
        Vector3 eulerBase = aimTransform.eulerAngles;
        aimTransform.eulerAngles = eulerBase - eulerDelta;
        aimTransform.DORotate(2 * eulerDelta, attackLength, RotateMode.WorldAxisAdd).SetEase(Ease.InOutQuad).OnComplete(onAttackEnd);
        if (hitting) return;
        //StartCoroutine(nameof(Hit));

        //Physics2D.Cast

        //ObjectManager.dictObjectToBreakable[other.gameObject].Hit(damage, status.none, critical);
    }

    public void onHit(Collider2D other)
    {
        HitInfo hitInfo = new HitInfo(stats);
        ObjectManager.dictObjectToHitable[other.gameObject].Hit(hitInfo);
    }

    private void onAttackEnd()
    {
        swordCollider.enabled = false;
        player.ReleaseWeaponRotation();
    }

    /*IEnumerator Hit() {
        hitting = true;

        int damage;
        bool critical;
        status effect = status.none;
        getDamage(out damage, out critical, out effect)

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

        yield return Helpers.GetWait(stats.cooldown);
        hitting = false;
    }*/
    
}
