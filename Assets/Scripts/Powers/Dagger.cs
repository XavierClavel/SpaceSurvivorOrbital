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
        Debug.Log(currentAngle);
        Bullet dagger = pool.get(playerTransform.position + Vector3.back,  currentAngle * Vector3.forward);
        currentAngle -= angleDeviation;
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
