using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * <pre>
 * <p> BaseDamage -> Ice Spike damage </p>
 * <p> IntA -> Amount of damage absorbed </p>
 * </pre>
 */
public class PowerIceSpike : Power, IPlayerEvents
{
    private const float rotationRandomizationFactor = 10f;
    private const float shieldReloadCooldown = 15f;
    
    public static ComponentPool<IceSpike> pool;
    [SerializeField] private IceSpike iceSpikePrefab;
    private static PowerIceSpike instance;
    private int shieldMaxCharges;
    private int shieldCharges;

    public override void onSetup()
    {
        instance = this;
        autoCooldown = true;
        pool = new ComponentPool<IceSpike>(iceSpikePrefab);
        shieldMaxCharges = fullStats.generic.intA;
        shieldCharges = shieldMaxCharges;
        if (shieldCharges > 0) EventManagers.player.registerListener(this);
    }

    private void OnDestroy()
    {
        EventManagers.player.unregisterListener(this);
    }

    protected override void onUse()
    {
        StartCoroutine(nameof(SpawnIceSpikes));
    }

    private IEnumerator RechargeShield()
    {
        while (shieldCharges < shieldMaxCharges)
        {
            yield return Helpers.getWait(shieldReloadCooldown);
            shieldCharges++;
        }
    }

    private IEnumerator SpawnIceSpikes()
    {
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
            .setup(2f, 1f)
            ;
    }

    public static void HitEnnemy(GameObject ennemy)
    {
        HitInfo hitInfo = new HitInfo(Power.getDamage(instance.stats.baseDamage), false, status.ice);
        SoundManager.PlaySfx(ennemy.transform, key: "Ennemy_Hit");
        ObjectManager.HitObject(ennemy, hitInfo);
    }

    public bool onPlayerDeath()
    {
        return false;
    }

    public bool onPlayerHit(bool shieldHit)
    {
        if (shieldCharges == 0) return false;
        shieldCharges--;
        if (shieldCharges == shieldMaxCharges - 1) StartCoroutine(nameof(RechargeShield));
        return true;
    }
}
