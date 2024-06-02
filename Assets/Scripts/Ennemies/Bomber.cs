using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class Bomber : Ennemy
{
    enum state { exploding, approaching }
    state ennemyState = state.approaching;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionTriggerDistance;
    [SerializeField] float timeBeforeExplosion;
    [SerializeField] ParticleSystem explosionPS;
    WaitForSeconds waitExplosion;
    float sqrExplosionTriggerDistance;
    float currentSpeed;
    int playerLayer;

    protected override void Start()
    {
        base.Start();
        
        doUseKillPs = false;
        waitExplosion = Helpers.getWait(timeBeforeExplosion);
        sqrExplosionTriggerDistance = Mathf.Pow(explosionTriggerDistance, 2);
        playerLayer = LayerMask.GetMask(Vault.layer.Player);


        StartCoroutine(nameof(SwitchState));
    }

    IEnumerator SwitchState()
    {
        while (true)
        {
            yield return waitStateStep;

            if (distanceToPlayer.sqrMagnitude < sqrExplosionTriggerDistance)
            {
                ennemyState = state.exploding;
                //rb.velocity = Vector2.zero;
                DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, 0.1f).SetEase(Ease.InQuad);
                yield return waitExplosion;
                Explode();
                break;
            }


            ennemyState = state.approaching;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad);
        }
    }

    private void Explode()
    {
        ShockwaveManager.SpawnShockwave("Bomber", transform.position, explosionRadius);
        //var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        //if (hits.Length > 0) PlayerController.Hurt(baseDamage);
        Death();
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
