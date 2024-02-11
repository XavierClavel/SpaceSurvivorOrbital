
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dropper : Ennemy
{
    [SerializeField] private int resourcesAmount = 5;
    private const float fleeUntil = 10f;

    state ennemyState = state.approaching;
    float currentSpeed;
    float sqrFleeRange;

    protected override void Start()
    {
        base.Start();

        sqrFleeRange = Mathf.Pow(fleeUntil, 2);
        fleeSpeed = 4.5f;

        StartCoroutine(nameof(SwitchState));
    }

    IEnumerator SwitchState()
    {
        while (true)
        {
            yield return waitStateStep;
            float sqrDistance = distanceToPlayer.sqrMagnitude;

            if (sqrDistance < sqrFleeRange)
            {
                ennemyState = state.fleeing;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, fleeSpeed, 0.5f).SetEase(Ease.InQuad);
                continue;
            }


            ennemyState = state.approaching;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad); ;
            continue;
        }
    }

    protected override void FixedUpdate()
    //TODO : run on lower frequency
    {
        if (knockback) return;
        base.FixedUpdate();
        switch (ennemyState)
        {
            case state.fleeing:
                Move(-directionToPlayer, currentSpeed);
                break;

            case state.approaching:
                Move(directionToPlayer, currentSpeed);
                break;
        }
    }

    protected override void onDeath()
    {
        ObjectManager.SpawnResources(transform.position,resourcesAmount);
    }
}
