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

    protected override void Start()
    {
        base.Start();
        healRangeDisplay.localScale = range * Vector3.one;
        mask = LayerMask.GetMask(Vault.layer.Ennemies);

        sqrFleeRange = Mathf.Pow(healRange.x, 2);
        sqrHealRange = Mathf.Pow(healRange.y, 2);

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


    protected override void FixedUpdate()
    //TODO : run on lower frequency
    {
        if (knockback) return;
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
                if (needsToRecharge) StartCoroutine(nameof(Recharge));
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, mask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject) continue; //Does not heal himself
            Ennemy ennemy = ObjectManager.dictObjectToEnnemy[collider.gameObject];
            if (ennemy.GetType() != this.GetType()) ennemy.HealSelf(baseDamage.getRandom());    //does not heal other healers
        }
        needsToRecharge = true;
        StartCoroutine(nameof(Recharge));
    }
}
