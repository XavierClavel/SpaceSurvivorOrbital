using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Ennemy
{
    private ComponentPool<Bullet> poolBullets;
    [SerializeField] private Bullet bulletPrefab;
    private const float bulletSpeed = 6f;
    private const float bulletLifetime = 5f;
    private const int amountMultiBullets = 16;
    private float waveSpread;

    protected override void Start()
    {
        base.Start();
        isImmuneToEffects = true;
        waveSpread = 360f / amountMultiBullets;
        poolBullets = new ComponentPool<Bullet>(bulletPrefab);
        InvokeRepeating(nameof(FireTowardsPlayer),2f,2f);
        InvokeRepeating(nameof(FireBulletsWave), 5f, 10f);
    }

    private void FireTowardsPlayer()
    {
        FireBullets(transform.getRotationTo(player.transform).z, 1,waveSpread);
    }

    private void FireBulletsWave()
    {
        FireBullets(0f, amountMultiBullets,waveSpread);
    }

    private void FireBullets(float rotation, int amountBullets, float spread)
    {
        int sideBullets = amountBullets / 2;

        if (amountBullets % 2 == 1)
        {
            for (int i = -sideBullets; i <= sideBullets; i++)
            {
                FireBullet(rotation + i * spread);
            }
            return;
        }

        for (int i = -sideBullets; i <= sideBullets; i++)
        {
            if (i == 0) continue;
            float j = i - Mathf.Sign(i) * 0.5f;

            FireBullet(rotation + j * spread);
        }
    }
    
    private void FireBullet(float rotation)
    {
        poolBullets.get(transform.position + Vector3.back,  rotation * Vector3.forward)
            .setPool(poolBullets)
            .Fire(bulletSpeed, bulletLifetime, baseDamage.getRandom());
    }
}
