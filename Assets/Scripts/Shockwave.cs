using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Shapes;
using UnityEngine;
using UnityEngine.Events;

public class Shockwave : MonoBehaviour
{
    private const float shockwaveDelay = 1f;
    private Disc disc;
    private Color? baseShockwaveColor = null;
    private Color clearColor;
    private float shockwaveDuration = 1f;
    private int shockwaveDamage;
    private float shockwaveRange;
    private int shockwaveKnockback;
    private status effect;
    private List<GameObject> objectsHit;
    private UnityAction recallAction = null;
    
    public void Setup(float shockwaveRange, int shockwaveDamage, status effect, int shockwaveKnockback)
    {
        this.shockwaveRange = shockwaveRange;
        this.shockwaveDamage = shockwaveDamage;
        this.shockwaveKnockback = shockwaveKnockback;
        this.effect = effect;
        
        disc = GetComponent<Disc>();
        baseShockwaveColor ??= disc.Color;
        clearColor = (Color)baseShockwaveColor;
        clearColor.a = 0;
    }
    
    public void setRecallMethod(UnityAction action)
    {
        recallAction = action;
    }

    public void doShockwave(bool destroyOnComplete = false)
    {
        objectsHit = new List<GameObject>();
        if (baseShockwaveColor != null) disc.Color = (Color)baseShockwaveColor;
        transform.localScale = Vector3.zero;
        
        transform.DOScale(shockwaveRange, shockwaveDuration).OnComplete(
            delegate
            {
                transform.localScale = Vector3.zero;
                if (!destroyOnComplete) return;
                
                if (recallAction != null) Invoke(nameof(Recall), shockwaveDelay);
                else GameObject.Destroy(gameObject, shockwaveDelay);
            });
        DOTween.To(() => disc.Color, x => disc.Color = x, clearColor, shockwaveDuration).SetEase(Ease.OutQuad);
    }

    private void Recall()
    {
        recallAction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (objectsHit == null) return;
        if (objectsHit.Contains(other.gameObject)) return;
        objectsHit.Add(other.gameObject);
        
        HitInfo hitInfo = new HitInfo(shockwaveDamage, false, effect, shockwaveKnockback);
        ObjectManager.dictObjectToHitable[other.gameObject].Hit(hitInfo);
        
    }
}
