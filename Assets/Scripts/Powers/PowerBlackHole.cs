using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <pre>
 * <p> AttackSpeed -> Black hole duration </p>
 * <p> Cooldown -> Delay between black holes </p>
 * <p> Projectiles -> Amount of black holes </p>
 * <p> Range -> Scale of black holes </p>
 * <p> Knockback -> Attraction force </p>
 * <p> BaseDamage -> Black hole dps </p>
 * </pre>
 */
public class PowerBlackHole : Power
{
    [SerializeField] private BlackHole prefabBlackHole;
    private ComponentPool<BlackHole> pool;
    private Camera cam;
    private static PowerBlackHole instance;

    private float blackHoleDuration;
    private float blackHoleSize;
    private float blackHoleForce;
    private float blackHoleDps;

    public override void onSetup()
    {
        cam = Camera.main;
        instance = this;
        autoCooldown = true;
        pool = new ComponentPool<BlackHole>(prefabBlackHole);

        blackHoleDuration = stats.attackSpeed;
        blackHoleSize = stats.range;
        blackHoleForce = stats.knockback;
        blackHoleDps = stats.baseDamage.getRandom();
        
        Debug.Log(blackHoleSize);
    }

    protected override void onUse()
    {
        BlackHole blackHole = pool.get(cam);
        blackHole.setup(blackHoleSize, blackHoleDuration, blackHoleForce, blackHoleDps);
    }
    
    public static void recall(BlackHole blackHole)
    {
        instance.pool.recall(blackHole);
    }
}
