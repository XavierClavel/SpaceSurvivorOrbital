using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Ennemy
{
    private ComponentPool<Bullet> poolBullets;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] ParticleSystem waveExplosionPS;
    private const float bulletSpeed = 6f;
    private const float bulletLifetime = 5f;
    private const int amountMultiBullets = 16;
    private float waveSpread;
    private const float bulletSpread = 10f;
    private SpriteRenderer spriteRenderer;
    private float timeBeforeWave = 10f;
    private bool waveDone;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxHealth = 1000 * PlanetSelector.getDifficulty();
        health = maxHealth;
        healthBar = ObjectManager.getBossHealthbar();
        healthBar.gameObject.SetActive(true);
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        isImmuneToEffects = true;
        waveSpread = 360f / amountMultiBullets;
        poolBullets = new ComponentPool<Bullet>(bulletPrefab);
        InvokeRepeating(nameof(FireTowardsPlayer),2f,1f);
        InvokeRepeating(nameof(FireBulletsWave), timeBeforeWave, 10f);
        StartCoroutine(nameof(waitPS));
    }

    IEnumerator waitPS()
    {
        yield return new WaitForSeconds(8);
        waveExplosionPS.Play();
    }

    private void FireTowardsPlayer()
    {
        
        FireBullets(transform.getRotationTo(player.transform).z, health > maxHealth * 0.5f ? 1 : 3,  bulletSpread);
    }

    private void FireBulletsWave()
    {
        FireBullets(0f, amountMultiBullets,waveSpread);
        waveExplosionPS.Stop();
        StartCoroutine(nameof(waitPS));
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
        poolBullets.get(transform.position + Vector3.back + new Vector3 (0, -2, 0),  rotation * Vector3.forward)
            .setPool(poolBullets)
            .Fire(bulletSpeed, bulletLifetime, baseDamage.getRandom());
    }
}
