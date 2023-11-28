using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Shapes;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Disc disc;
    private Color baseShockwaveColor;
    private Color clearColor;
    private float shockwaveDuration = 1f;
    private int shockwaveDamage;
    private float shockwaveRange;
    private status effect;
    private List<GameObject> objectsHit;
    
    public void Setup(float shockwaveRange, int shockwaveDamage, status effect)
    {
        this.shockwaveRange = shockwaveRange;
        this.shockwaveDamage = shockwaveDamage;
        
        disc = GetComponent<Disc>();
        baseShockwaveColor = disc.Color;
        clearColor = baseShockwaveColor;
        clearColor.a = 0;

        this.effect = effect;
    }

    public void doShockwave(bool destroyOnComplete = false)
    {
        objectsHit = new List<GameObject>();
        disc.Color = baseShockwaveColor;
        transform.localScale = Vector3.zero;
        
        transform.DOScale(shockwaveRange, shockwaveDuration).OnComplete(
            delegate
            {
                transform.localScale = Vector3.zero;
                if (destroyOnComplete) GameObject.Destroy(gameObject, 1f);
            });
        DOTween.To(() => disc.Color, x => disc.Color = x, clearColor, shockwaveDuration).SetEase(Ease.OutQuad);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (objectsHit.Contains(other.gameObject)) return;
        objectsHit.Add(other.gameObject);
        
        HitInfo hitInfo = new HitInfo(shockwaveDamage, false, effect);
        ObjectManager.dictObjectToHitable[other.gameObject].Hit(hitInfo);
        
    }
}
