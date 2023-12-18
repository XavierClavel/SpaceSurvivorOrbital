using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToxicZone : Power
{
    private int dps;

    public void Setup(int dps, float scale)
    {
        this.dps = dps;
        
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scale * Vector3.one, 0.2f));
        sequence.AppendInterval(5f);
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.OnComplete(delegate { PowerToxicZone.recall(this); });
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ObjectManager.dictObjectToEnnemy.ContainsKey(other.gameObject))
        {
            ObjectManager.dictObjectToEnnemy[other.gameObject].StackDamage(dps, status.none);
        }
    }
}
