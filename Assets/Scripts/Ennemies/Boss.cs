using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
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
    state ennemyState = state.approaching;
    Vector2 currentDir;
    float currentSpeed;
    float sqrFleeRange;
    float sqrShootRange;
    Vector2 shootRange = new Vector2(3f, 5f);

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
        isImmuneToKnockback = true;
        waveSpread = 360f / amountMultiBullets;
        poolBullets = new ComponentPool<Bullet>(bulletPrefab);
        speed *= 50f;
        
        sqrFleeRange = Mathf.Pow(shootRange.x, 2);
        sqrShootRange = Mathf.Pow(shootRange.y, 2);
        
        onDeath.AddListener(ObjectManager.DisplaySpaceship);
        onDeath.AddListener(delegate { Spaceship.setDestination(gameScene.win); });
        
        StartCoroutine(nameof(BossController));
        StartCoroutine(nameof(SwitchState));
    }

    private IEnumerator BossController()
    {
        int counter = 0;
        while (true)
        {
            yield return Helpers.getWait(1f);
            counter++;
            if (counter == 10)
            {
                waveExplosionPS.Play();
                yield return Helpers.getWait(2f);
                FireBulletsWave();
                counter = 0;
            }
            else
            {
                FireTowardsPlayer();
            }
        }
    }

    private void FireTowardsPlayer()
    {
        Helpers.FireProjectiles(
            FireBullet,
            health > maxHealth * 0.5f ? 1 : 3, 
            bulletSpread,
            transform.getRotationTo(player.transform).z
            );
    }

    private void FireBulletsWave()
    {
        SoundManager.PlaySfx(transform, key: "Boss_WaveShoot");
        Helpers.FireProjectiles(FireBullet, amountMultiBullets, waveSpread, 0f);
        waveExplosionPS.Stop();
    }

    private void FireBullet(float rotation)
    {
        SoundManager.PlaySfx(transform, key: "Boss_Shoot");
        poolBullets
            .get(transform.position + Vector3.back + new Vector3 (0, -2, 0),  rotation * Vector3.forward)
            .setPool(poolBullets)
            .setTimer(bulletLifetime)
            .Fire(bulletSpeed, bulletLifetime, baseDamage.getRandom());
    }
    
    protected override void FixedUpdate()
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
                break;

            case state.approaching:
                Move(directionToPlayer, currentSpeed);
                currentDir = directionToPlayer;
                break;
        }
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
                DOTween.To(() => currentSpeed, x => currentSpeed = x, fleeSpeed, 0.5f).SetEase(Ease.InQuad);
                continue;
            }

            if (sqrDistance < sqrShootRange)
            {
                ennemyState = state.shooting;
                DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, 0.5f).SetEase(Ease.InQuad); ;
                continue;

            }

            ennemyState = state.approaching;
            DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, 0.5f).SetEase(Ease.InQuad); ;
            continue;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Vault.tag.Player))
        {
            PlayerController.Hurt(baseDamage);
            ApplyKnockback(200, true);
        }
    }
}
