using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum state { fleeing, shooting, approaching }

public class Gunner : Ennemy
{


    private int bulletSpeed = 5;
    [SerializeField] Vector2 shootRange = new Vector2(3f, 5f);

    [Header("Additional References")]
    public Bullet bulletPrefab;
    bool shooting = false;

    bool needsToReload = true;
    bool reloading = false;
    float lifetime;
    state ennemyState = state.approaching;
    Vector2 currentDir;
    float currentSpeed;
    float sqrFleeRange;
    float sqrShootRange;

    protected override void Start()
    {
        base.Start();
        lifetime = range / (float)bulletSpeed;

        sqrFleeRange = Mathf.Pow(shootRange.x, 2);
        sqrShootRange = Mathf.Pow(shootRange.y, 2);

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
                shooting = false;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, fleeSpeed, 0.5f).SetEase(Ease.InQuad);
                continue;
            }

            if (sqrDistance < sqrShootRange)
            {
                ennemyState = state.shooting;
                shooting = true;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad); ;
                continue;

            }

            ennemyState = state.approaching;
            shooting = false;
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

            case state.shooting:
                Move(currentDir, currentSpeed);
                if (reloading) break;
                if (needsToReload) StartCoroutine(nameof(Reload));
                else Shoot();
                break;

            case state.approaching:
                Move(directionToPlayer, currentSpeed);
                currentDir = directionToPlayer;
                break;
        }
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return wait;
        reloading = false;
        needsToReload = false;
        if (shooting) Shoot();
    }

    void Shoot()
    {
        //soundManager.PlaySfx(transform, sfx.ennemyShoots);
        Quaternion rotation = Helpers.LookAt2D(transform.position, player.transform.position);
        Bullet bullet = Instantiate(bulletPrefab, transform.position, rotation);
        bullet.gameObject.SetActive(true);
        bullet.Fire(bulletSpeed, lifetime, baseDamage.getRandom());
        needsToReload = true;
        StartCoroutine(nameof(Reload));
    }
}
