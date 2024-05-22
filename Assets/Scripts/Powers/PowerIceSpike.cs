using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIceSpike : Power
{
    public static ComponentPool<IceSpike> pool;
    [SerializeField] private IceSpike iceSpikePrefab;
    private static PowerIceSpike instance;
    private const float rotationRandomizationFactor = 10f;

    public override void onSetup()
    {
        instance = this;
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
        Vector2 direction = player.moveDir;
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
        float randomRotation = Random.Range(-rotationRandomizationFactor, rotationRandomizationFactor);
        pool
            .get(position,  (90 + randomRotation) * Vector3.forward)
            .setup(stats.baseDamage, 2f, 1f)
            ;
    }

    public static void HitEnnemy(GameObject ennemy)
    {
        HitInfo hitInfo = new HitInfo(Power.getDamage(instance.stats.baseDamage), false, status.ice);
        SoundManager.PlaySfx(ennemy.transform, key: "Ennemy_Hit");
        ObjectManager.HitObject(ennemy, hitInfo);
    }
}
