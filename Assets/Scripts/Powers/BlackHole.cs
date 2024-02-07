using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private float forceAttraction;

    public void setup(float scale, float lifetime, float force)
    {
        this.forceAttraction = force;
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
        ennemy.ApplyForce(CalculateForce(ennemy.transform));
    }

    private Vector2 CalculateForce(Transform t)
    {
        Vector3 direction = transform.position - t.position;
        float force = forceAttraction / direction.sqrMagnitude;
        return direction * force;
    }
}
