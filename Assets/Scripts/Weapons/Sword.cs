using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/**
 * 
 */
public class Sword : Interactor
{
    [SerializeField] private ParticleSystem slashPs;

    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        slashPs.transform.localScale = stats.range * 0.33f * Vector3.one;
    }

    protected override void onStartUsing() { }

    protected override void onStopUsing() { }

    protected override void onUse()
    {
        var hits = Helpers.OverlapCircularArcAll(player.transform, aimTransform.right, stats.range, stats.spread, currentLayerMask);
        hits.ForEach(onHit);
        slashPs.Play();
        StartCoroutine(nameof(HideSword));

    }

    private IEnumerator HideSword()
    {
        spriteRenderer.enabled = false;
        yield return Helpers.getWait(0.2f);
        spriteRenderer.enabled = true;
    }

    public void onHit(Collider2D other)
    {
        HitInfo hitInfo = new HitInfo(stats);
        hitInfo.ApplyBonus();
        hitInfo.addDamageMultiplier();
        ObjectManager.retrieveHitable(other.gameObject)?.Hit(hitInfo);
    }

}
