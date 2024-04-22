using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    private Vector2Int baseDamage;
    private float lifetime;
    private float scale;

    public IceSpike setup(Vector2Int baseDamage, float lifetime, float scale)
    {
        this.baseDamage = baseDamage;
        this.lifetime = lifetime;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scale * Vector3.one, 0.2f));
        sequence.AppendInterval(lifetime);
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.OnComplete(delegate
        {
            PowerIceSpike.pool.recall(this);
        });
        
        return this;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Vault.tag.Ennemy)) return;
        PowerIceSpike.HitEnnemy(other.gameObject);
    }
}
