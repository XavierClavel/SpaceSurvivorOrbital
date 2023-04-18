using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Healer : Ennemy
{
    enum state { fleeing, healing, approaching }
    state ennemyState = state.approaching;

    bool healing = false;

    bool needsToRecharge = true;
    bool recharging = false;

    [SerializeField] Vector2 healRange = new Vector2(3f, 5f);

    [Header("Additional References")]
    [SerializeField] Transform healRangeDisplay;
    LayerMask mask;
    Vector2 currentDir;
    float currentSpeed;
    float sqrFleeRange;
    float sqrHealRange;

    internal override void Start()
    {
        base.Start();
        healRangeDisplay.localScale = range * Vector3.one;
        mask = LayerMask.GetMask("Ennemies");

        sqrFleeRange = Mathf.Pow(healRange.x, 2);
        sqrHealRange = Mathf.Pow(healRange.y, 2);

        StartCoroutine("SwitchState");
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
                healing = false;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, fleeSpeed, 0.5f).SetEase(Ease.InQuad);
                continue;
            }

            if (sqrDistance < sqrHealRange)
            {
                ennemyState = state.healing;
                healing = true;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad); ;
                continue;
            }


            ennemyState = state.approaching;
            healing = false;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad); ;
            continue;
        }
    }


    internal override void FixedUpdate()
    //TODO : run on lower frequency
    {
        base.FixedUpdate();
        switch (ennemyState)
        {
            case state.fleeing:
                Move(-directionToPlayer, currentSpeed);
                currentDir = -directionToPlayer;
                break;

            case state.healing:
                Move(currentDir, currentSpeed);
                if (recharging) break;
                if (needsToRecharge) StartCoroutine("Recharge");
                else Heal();
                break;

            case state.approaching:
                Move(directionToPlayer, currentSpeed);
                currentDir = directionToPlayer;
                break;
        }
    }

    IEnumerator Recharge()
    {
        recharging = true;
        yield return wait;
        recharging = false;
        needsToRecharge = false;
        if (healing) Heal();
    }

    void Heal()
    {
        Debug.Log("heal");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.up, 0.00001f, mask);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue; //Does not heal himself
            Planet.dictObjectToEnnemy[hit.collider.gameObject].HealSelf(baseDamage);
        }
        needsToRecharge = true;
        StartCoroutine("Recharge");
    }
}
