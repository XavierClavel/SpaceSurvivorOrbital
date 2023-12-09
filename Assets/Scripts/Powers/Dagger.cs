using System.Collections;
using System.Collections.Generic;
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
    private ComponentPool<Bullet> pool;
    private ComponentPool<Shockwave> shockwavePool;
    private float currentAngle = 0f;
    const float angleDeviation = 10f;
    private List<Bullet> daggersStack = new List<Bullet>();
    private bool doMirror;
    private bool doRecall;
    private bool doExplode;

    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        doMirror = fullStats.generic.boolA;
        doRecall = fullStats.generic.boolB;
        doExplode = fullStats.generic.boolC;
        doMirror = true;
        doExplode = true;
        
        autoCooldown = true;
        daggerPrefab = Instantiate(daggerPrefab);
        daggerPrefab.pierce = stats.pierce;
        daggerPrefab.gameObject.SetActive(false);
        pool = new ComponentPool<Bullet>(daggerPrefab);
        shockwavePool = new ComponentPool<Shockwave>(daggerShockwave);
    }

    protected override void onUse()
    {
        FireDaggers(currentAngle);
        if (doMirror) FireDaggers(currentAngle + 180f);
        currentAngle -= angleDeviation;
    }

    private void FireDaggers(float rotation)
    {
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
        dagger.setImpactAction(delegate(Bullet bullet)
        {
            Debug.Log("Explosion");
            Shockwave shockwave = shockwavePool.get(bullet.transform.position);
            shockwave.Setup(2f, 15, status.none, 0);
            shockwave.doShockwave();
        });
        daggersStack.Add(dagger);
        Invoke(nameof(recallDagger), 1f);
    }

    private void recallDagger()
    {
        Bullet dagger = daggersStack.Pop();
        dagger.gameObject.SetActive(false);
        pool.recall(dagger);
    }
}
