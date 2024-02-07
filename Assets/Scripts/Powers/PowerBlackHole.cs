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
 * <p> BoolA -> Whether to spawn white hole </p>
 * <p> BoolB -> Whether black holes can be traversed </p>
 * </pre>
 */
public class PowerBlackHole : Power
{
    [SerializeField] private BlackHole prefabBlackHole;
    [SerializeField] private WhiteHole prefabWhiteHole;
    private ComponentPool<BlackHole> pool;
    private ComponentPool<WhiteHole> poolWhite;
    private Camera cam;
    private static PowerBlackHole instance;

    private float blackHoleDuration;
    private float blackHoleSize;
    private float blackHoleForce;
    private float blackHoleDps;
    private bool doSpawnWhiteHole;
    private bool doWormHole;

    private BlackHole blackHole;
    private WhiteHole whiteHole;

    public override void onSetup()
    {
        cam = Camera.main;
        instance = this;
        autoCooldown = true;
        pool = new ComponentPool<BlackHole>(prefabBlackHole);
        poolWhite = new ComponentPool<WhiteHole>(prefabWhiteHole);

        blackHoleDuration = stats.attackSpeed;
        blackHoleSize = stats.range;
        blackHoleForce = stats.knockback;
        blackHoleDps = stats.baseDamage.getRandom();
        doSpawnWhiteHole = fullStats.generic.boolA;
        doWormHole = fullStats.generic.boolB;
    }

    protected override void onUse()
    {
        blackHole = pool.get(cam);
        blackHole.setup(blackHoleSize, blackHoleDuration, blackHoleForce, blackHoleDps);

        if (!doSpawnWhiteHole)
        {
            return;
        }

        whiteHole = poolWhite.get(cam);
        whiteHole.setup(blackHoleSize, blackHoleDuration, blackHoleForce, blackHoleDps);
    }
    
    public static void recall(BlackHole blackHole)
    {
        instance.pool.recall(blackHole);
    }
    
    public static void recall(WhiteHole whiteHole)
    {
        instance.poolWhite.recall(whiteHole);
    }

    public static void TraverseBlackHole()
    {
        if (!instance.doWormHole)
        {
            return;
        }
        PlayerController.instance.transform.position =
            instance.whiteHole.transform.position + Helpers.getRandomPositionInRing(new Vector2(1f, 1f), shape.square);
        instance.blackHole.Remove();
        instance.whiteHole.Remove();
    }
}
