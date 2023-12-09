using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Power
{
    [SerializeField] private Bullet daggerPrefab;
    private ComponentPool<Bullet> pool;
    private float currentAngle = 0f;
    const float angleDeviation = 10f;
    private List<Bullet> daggersStack = new List<Bullet>();

    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        autoCooldown = true;
        daggerPrefab = Instantiate(daggerPrefab);
        daggerPrefab.pierce = stats.pierce;
        daggerPrefab.gameObject.SetActive(false);
        pool = new ComponentPool<Bullet>(daggerPrefab);
    }

    protected override void onUse()
    {
        FireDaggers(currentAngle);
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
