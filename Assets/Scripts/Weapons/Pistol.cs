using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int projectiles = 1;
    [SerializeField] float distanceOffsetBetweenBullets = 0.1f;
    [SerializeField] float spread = 0f;
    float angleOffsetBetweenBullets = 0f;

    protected override void Start()
    {
        base.Start();
        if (projectiles > 1) angleOffsetBetweenBullets = spread / (projectiles - 1);
        bulletPrefab.pierce = pierce;
    }

    protected override void Fire()
    {
        if (projectiles == 1)
        {
            FireBullet(firePoint.position, firePoint.eulerAngles);
            return;
        }

        int sideProjectiles = projectiles / 2;

        if (projectiles % 2 == 1)
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
        Vector3 eulerRotation = firePoint.eulerAngles + i * angleOffsetBetweenBullets * Vector3.forward;
        FireBullet(position, eulerRotation);
    }

    void FireBullet(Vector3 position, Vector3 eulerRotation)
    {
        int damage = Random.Range(baseDamage.x, baseDamage.y + 1);
        bool critical = Random.Range(0f, 1f) < criticalChance;
        if (critical) damage = (int)((float)damage * criticalMultiplier);

        soundManager.PlaySfx(transform, sfx.shoot);

        bulletPrefab.damage = damage;
        bulletPrefab.critical = critical;
        bulletPrefab.effect = player.effect;


        Bullet bullet = Instantiate(bulletPrefab, position, Quaternion.Euler(eulerRotation));
        bullet.Fire(attackSpeed, bulletLifetime);


    }
}
