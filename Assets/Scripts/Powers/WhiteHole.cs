using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WhiteHole : MonoBehaviour
{
    private float scale;
    private float forceAttraction;
    private float dps;
    private HashSet<status> elements = new HashSet<status>();

    private const float damageSqrRadius = 3f;

    public void setup(float scale, float lifetime, float force, float damage)
    {
        this.forceAttraction = force;
        this.dps = damage;
        this.scale = scale;
        
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scale * Vector3.one, 0.2f));
        sequence.AppendInterval(lifetime);
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.OnComplete(delegate
        {
            PowerBlackHole.recall(this);
        });
    }


    void OnTriggerStay2D(Collider2D other)
    {
        // Checker si l'objet a le tag cible
        if (!other.CompareTag(Vault.tag.Ennemy)) return;
        if (!ObjectManager.dictObjectToEnnemy.ContainsKey(other.gameObject))
        {
            return;
        }
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
        
        Vector3 direction = transform.position - ennemy.transform.position;
        float force = forceAttraction / Mathf.Clamp(direction.sqrMagnitude, 0.0001f, 1000f);
        
        ennemy.ApplyForce(- direction * force);
        
        //Apply dps only if ennemy is at most half radius distance from the center
        if (direction.sqrMagnitude < 0.25 * Mathf.Pow(scale,2))
        {
            ennemy.StackDamage(dps, elements);
        }
        
    }
    
    public void Remove()
    {
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
        {
            PowerBlackHole.recall(this);
        });
    }

}