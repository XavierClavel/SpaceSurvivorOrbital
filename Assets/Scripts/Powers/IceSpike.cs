using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    private float scale;
    [SerializeField] private List<ParticleSystem> alignParallel;
    [SerializeField] private List<ParticleSystem> alignOrthogonal;
    [SerializeField] private ParticleSystem ps;

    public IceSpike setup(float lifetime, float scale)
    {
        alignOrthogonal.ForEach(it =>
        {
            var mainModule = it.main;
            mainModule.startRotation = this.transform.eulerAngles.z + 90;
            it.Play();
        });
        alignParallel.ForEach(it =>
        {
            var mainModule = it.main;
            mainModule.startRotation = this.transform.eulerAngles.z;
        });
        Debug.Log(alignOrthogonal[0].gameObject.name);
        Debug.Log(alignOrthogonal[0].startRotation);
        ps.Play();
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
