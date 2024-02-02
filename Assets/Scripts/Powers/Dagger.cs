using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


/**
 * <pre>
 * <p> BaseDamage -> Dagger damage </p>
 * <p> Cooldown -> Delay between daggers </p>
 * <p> Projectiles -> Amount of daggers </p>
 * <p> BoolA -> Whether effect is mirrored </p>
 * <p> BoolB -> Whether daggers are recalled </p>
 * <p> BoolC -> Whether daggers explode on impact </p>
 * </pre>
 */
public class Dagger : Power
{
    [SerializeField] private Bullet daggerPrefab;
    [SerializeField] private Shockwave daggerShockwave;
    [SerializeField] public ParticleSystem explosion;
    private ComponentPool<Bullet> pool;
    private ComponentPool<Shockwave> shockwavePool;
    private ComponentPool<ParticleSystem> explosionPool;
    private float currentAngle = 0f;
    const float angleDeviation = 30f;
    private bool doMirror = false;
    private bool doRecall;
    private bool doExplode;
    private float daggerDuration;

    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        doMirror = fullStats.generic.boolA;
        doRecall = fullStats.generic.boolB;
        doExplode = fullStats.generic.boolC;
        
        autoCooldown = true;
        daggerPrefab = Instantiate(daggerPrefab);
        daggerPrefab.pierce = stats.pierce;
        daggerPrefab.gameObject.SetActive(false);
        pool = new ComponentPool<Bullet>(daggerPrefab);
        shockwavePool = new ComponentPool<Shockwave>(daggerShockwave);
        explosionPool = new ComponentPool<ParticleSystem>(explosion);
        explosionPool.setTimer(1f);
        daggerDuration = Mathf.Min(Camera.main.getBounds().extents.x, Camera.main.getBounds().extents.y) * 0.95f / stats.attackSpeed;
    }

    protected override void onUse()
    {
        FireDaggers(currentAngle);
        if (doMirror) FireDaggers(currentAngle + 180f);
        currentAngle -= angleDeviation;
    }

    private void FireDaggers(float rotation)
    {
        SoundManager.PlaySfx(transform, key: "Dagger_Throw");

        if (stats.projectiles == 1)
        {
            FireDagger(rotation);
            return;
        }

        int sideProjectiles = stats.projectiles / 2;

        if (stats.projectiles % 2 == 1)
        {
            for (int i = -sideProjectiles; i <= sideProjectiles; i++)
            {
                FireDagger(rotation + i * stats.spread);
            }
            return;
        }

        for (int i = -sideProjectiles; i <= sideProjectiles; i++)
        {
            if (i == 0) continue;
            float j = i - Mathf.Sign(i) * 0.5f;

            FireDagger(rotation + j * stats.spread);
        }
        return;
    }

    private void FireDagger(float rotation)
    {
        Bullet dagger = pool.get(playerTransform.position + Vector3.back,  rotation * Vector3.forward);
        dagger.Fire(stats.attackSpeed, new HitInfo(stats));
        if (doExplode)
        {   

            dagger.setImpactAction(delegate(Bullet bullet)
            {
                explosionPool.get(bullet.transform.position).Play();
                SoundManager.PlaySfx(transform, key: "Ghost_Explode");
                Shockwave shockwave = shockwavePool.get(bullet.transform.position);
                shockwave.Setup(2f, 10, status.none, 0);
                shockwave.setRecallMethod(delegate { shockwavePool.recall(shockwave); });
                shockwave.doShockwave(true);
            });

        }

        StartCoroutine(recall(dagger));
    }

    private IEnumerator recall(Bullet dagger)
    {
        if (!doRecall)
        {
            yield return Helpers.getWait(daggerDuration);
            recallDagger(dagger);
            yield break;
        }
        
        yield return Helpers.getWait(daggerDuration);
        Vector2 velocity = dagger.rb.velocity;
        dagger.rb.velocity = Vector2.zero;
        dagger.transform.DORotate(180f * Vector3.forward, 0.1f).SetRelative();
        yield return Helpers.getWait(0.1f);
        dagger.rb.velocity = -velocity;
        yield return Helpers.getWait(daggerDuration);
        recallDagger(dagger);
    }

    private void recallDagger(Bullet dagger)
    {
        pool.recall(dagger);
    }
}
