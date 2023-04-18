using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gunner : Ennemy
{
    enum state { fleeing, shooting, approaching }

    [Header("Additional References")]
    public Bullet bulletPrefab;
    bool shooting = false;

    bool needsToReload = true;
    bool reloading = false;
    float lifetime;
    [SerializeField] int bulletSpeed = 5;
    state ennemyState = state.approaching;
    Vector2 currentDir;
    float currentSpeed;

    internal override void Start()
    {
        base.Start();
        lifetime = range / (float)bulletSpeed;
        StartCoroutine("SwitchState");
    }

    IEnumerator SwitchState()
    {
        while (true)
        {
            yield return waitStateStep;
            switch (distanceToPlayer.magnitude)
            {
                case < 3f:
                    ennemyState = state.fleeing;
                    shooting = false;
                    DOTween.To(() => currentSpeed, x => currentSpeed = x, fleeSpeed, 0.5f).SetEase(Ease.InQuad);
                    break;

                case < 5f:
                    ennemyState = state.shooting;
                    shooting = true;
                    DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad); ;
                    break;

                default:
                    ennemyState = state.approaching;
                    shooting = false;
                    DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad); ;
                    break;
            }
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

            case state.shooting:
                Move(currentDir, currentSpeed);
                if (reloading) break;
                if (needsToReload) StartCoroutine("Reload");
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
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Helpers.v2ToQuaternion(directionToPlayer));
        bullet.gameObject.SetActive(true);
        bullet.Fire(bulletSpeed, lifetime, baseDamage);
        needsToReload = true;
        StartCoroutine("Reload");
    }
}
