using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToxicZone : Power
{
    [SerializeField] private Rigidbody2D rb;
    private static float toxicZoneSpeed = 1f;

    public void Setup(float scale, bool doFollowPlayer)
    {
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scale * Vector3.one, 0.2f));
        sequence.AppendInterval(5f);
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.OnComplete(delegate
        {
            if (doFollowPlayer) StopCoroutine(nameof(FollowPlayer));
            PowerToxicZone.recall(this);
        });

        if (doFollowPlayer) StartCoroutine(nameof(FollowPlayer));
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            yield return Helpers.GetWaitFixed;
            rb.AddForce((playerTransform.position - transform.position).normalized * toxicZoneSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ObjectManager.dictObjectToEnnemy.ContainsKey(other.gameObject)) return;
        PowerToxicZone.OnEnnemyEnterToxicZone(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PowerToxicZone.OnEnnemyExitToxicZone(other.gameObject);
    }
}
