using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    [SerializeField] float distanceOffsetBetweenBullets = 0.1f;

    protected override void Start()
    {
        base.Start();
        bulletPrefab.pierce = stats.pierce;
    }

    protected override void Fire()
    {
        if (stats.projectiles == 1)
        {
            FireBullet(firePoint.position, firePoint.eulerAngles);
            return;
        }

        int sideProjectiles = stats.projectiles / 2;

        if (stats.projectiles % 2 == 1)
        {
            for (int i = -sideProjectiles; i <= sideProjectiles; i++)
            {
                FireBulletByIndex(i);
            }
            return;
        }

        for (int i = -sideProjectiles; i <= sideProjectiles; i++)
        {
            if (i == 0) continue;
            float j = (float)i - Mathf.Sign(i) * 0.5f;

            FireBulletByIndex(j);
        }
        return;
    }

    void FireBulletByIndex(float i)
    {
        Vector3 position = firePoint.position + i * distanceOffsetBetweenBullets * firePoint.right;
        Vector3 eulerRotation = firePoint.eulerAngles + i * stats.spread * Vector3.forward;
        FireBullet(position, eulerRotation);
    }

    void FireBullet(Vector3 position, Vector3 eulerRotation)
    {
        soundManager.PlaySfx(transform, sfx.shoot);
        Bullet bullet = pool.get(position, eulerRotation);
        bullet.Fire(stats.attackSpeed, bulletLifetime);
    }
}
