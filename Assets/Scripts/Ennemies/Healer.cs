using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Healer : Ennemy
{
    enum State { Fleeing, Healing, Approaching }
    State enemyState = State.Approaching;

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
        healRangeDisplay.localScale = healRange.y * Vector3.one;
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
                enemyState = State.Fleeing;
                healing = true;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, fleeSpeed, 0.5f).SetEase(Ease.InQuad);
                continue;
            }

            if (sqrDistance < sqrHealRange)
            {
                enemyState = State.Healing;
                healing = true;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad);
                continue;
            }

            enemyState = State.Approaching;
            healing = true;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad);
        }
    }

    protected override void FixedUpdate()
    {
        if (knockback) return;
        base.FixedUpdate();

        switch (enemyState)
        {
            case State.Fleeing:
                Move(-directionToPlayer, currentSpeed);
                currentDir = -directionToPlayer;
                break;

            case State.Healing:
                Move(currentDir, currentSpeed);
                if (recharging) break;
                if (needsToRecharge) StartCoroutine(nameof(Recharge));
                else Heal();
                break;

            case State.Approaching:
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRange.y, mask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == gameObject) continue; // Does not heal himself
            Ennemy enemy = ObjectManager.dictObjectToEnnemy[collider.gameObject];
            if (enemy.GetType() != this.GetType()) enemy.HealSelf(baseDamage.getRandom()); // Does not heal other healers
        }
        needsToRecharge = true;
        StartCoroutine(nameof(Recharge));
    }
}
