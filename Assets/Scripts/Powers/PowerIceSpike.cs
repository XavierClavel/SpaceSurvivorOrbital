using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIceSpike : Power
{
    public static ComponentPool<IceSpike> pool;
    [SerializeField] private IceSpike iceSpikePrefab;
    private static PowerIceSpike instance;

    public override void onSetup()
    {
        autoCooldown = true;
        pool = new ComponentPool<IceSpike>(iceSpikePrefab);
    }
    
    protected override void onUse()
    {
        StartCoroutine(nameof(SpawnIceSpikes));
    }

    private IEnumerator SpawnIceSpikes()
    {
        Debug.Log("Spawn");
        Vector2 startPos = playerTransform.position;
        Vector2 direction = player.aimVector;
        float range = 10f;
        int amount = 7;
        float step = range / amount;

        for (int i = 1; i <= amount; i++)
        {
            SpawnIceSpike(   startPos + i * step * direction);
            yield return Helpers.getWait(0.05f);
        }
    }

    private void SpawnIceSpike(Vector2 position)
    {
        pool
            .get(position)
            .setup(stats.baseDamage, 2f, 1f)
            ;
    }
}
