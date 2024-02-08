using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToxicZone : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private float toxicZoneSpeed = 1f;

    public void Setup(float scale, float speed)
    {
        this.toxicZoneSpeed = speed;
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(scale * Vector3.one, 0.2f));
        sequence.AppendInterval(30f);
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.OnComplete(delegate
        {
            if (toxicZoneSpeed > 0f) StopCoroutine(nameof(FollowPlayer));
            PowerToxicZone.recall(this);
        });

        if (toxicZoneSpeed > 0f) StartCoroutine(nameof(FollowPlayer));
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            yield return Helpers.getWaitFixed();
            Debug.Log((PlayerController.instance.transform.position - transform.position).normalized * toxicZoneSpeed);
            rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * toxicZoneSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PowerToxicZone.onPlayerEnterToxicZone();
            return;
        }
        if (!ObjectManager.dictObjectToEnnemy.ContainsKey(other.gameObject)) return;
        PowerToxicZone.onEnnemyEnterToxicZone(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PowerToxicZone.onPlayerExitToxicZone();
            return;
        }
        PowerToxicZone.onEnnemyExitToxicZone(other.gameObject);
    }

}
