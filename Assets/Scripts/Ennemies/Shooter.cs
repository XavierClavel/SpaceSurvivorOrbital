using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Ennemy
{
    //[SerializeField] float mean = 4f;
    //[SerializeField] float standardDeviation = 1f;

    [Header("Additional References")]
    public Bullet bulletPrefab;
    bool shooting = false;

    bool needsToReload = true;
    bool reloading = false;
    float lifetime;
    int bulletSpeed = 5;

    internal override void Start()
    {
        base.Start();
        lifetime = range / (float)bulletSpeed;
    }

    internal override void FixedUpdate()
    //TODO : run on lower frequency
    {
        base.FixedUpdate();
        switch (distanceToPlayer.magnitude)
        {
            case < 3f:
                shooting = false;
                Move(-directionToPlayer);
                break;

            case < 6f:
                shooting = true;
                if (reloading) break;
                if (needsToReload) StartCoroutine("Reload");
                else Shoot();
                break;

            default:
                shooting = false;
                Move(directionToPlayer);
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
