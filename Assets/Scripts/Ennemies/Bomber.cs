using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bomber : Ennemy
{
    enum state { exploding, approaching }
    state ennemyState = state.approaching;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionStartDistance;
    [SerializeField] float timeBeforeExplosion;
    [SerializeField] ParticleSystem explosionPS;
    WaitForSeconds waitExplosion;
    float sqrExplosionStartDistance;
    float currentSpeed;
    int playerLayer;

    protected override void Start()
    {
        base.Start();

        waitExplosion = Helpers.getWait(timeBeforeExplosion);
        sqrExplosionStartDistance = Mathf.Pow(explosionStartDistance, 2);
        playerLayer = LayerMask.GetMask(Vault.layer.Player);


        StartCoroutine(nameof(SwitchState));
    }

    IEnumerator SwitchState()
    {
        while (true)
        {
            yield return waitStateStep;
            float sqrDistance = distanceToPlayer.sqrMagnitude;

            if (sqrDistance < sqrExplosionStartDistance)
            {
                ennemyState = state.exploding;
                yield return waitExplosion;
                Helpers.SpawnPS(transform, explosionPS);
                //TODO : replace with overlapcircleall
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, explosionRadius, Vector2.zero, 1f, playerLayer);
                if (hit) PlayerController.Hurt(baseDamage);

                Destroy(gameObject);
                continue;
            }


            ennemyState = state.approaching;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad);
            continue;
        }
    }

    protected override void FixedUpdate()
    {
        if (knockback) return;
        base.FixedUpdate();
        switch (ennemyState)
        {
            case state.exploding:
                break;


            case state.approaching:
                Move(directionToPlayer, currentSpeed);
                break;
        }
    }

}
