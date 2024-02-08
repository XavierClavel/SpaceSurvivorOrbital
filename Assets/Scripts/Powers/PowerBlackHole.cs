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
 * <p> BoolC -> Explosion </p>
 * </pre>
 */
public class PowerBlackHole : Power
{
    [SerializeField] private BlackHole prefabBlackHole;
    [SerializeField] private WhiteHole prefabWhiteHole;
    [SerializeField] private Shockwave prefabShockwave;
    [SerializeField] ParticleSystem explosion;
    private ComponentPool<BlackHole> pool;
    private ComponentPool<WhiteHole> poolWhite;
    private ComponentPool<Shockwave> poolShockwaves;
    private ComponentPool<ParticleSystem> poolExplosion;
    private Camera cam;
    private static PowerBlackHole instance;

    private float blackHoleDuration;
    private float blackHoleSize;
    private float blackHoleForce;
    private float blackHoleDps;
    private bool doSpawnWhiteHole;
    private bool doWormHole;
    private bool doExplode;

    private BlackHole blackHole;
    private WhiteHole whiteHole;

    private const int shockwaveDamage = 20;
    

    public override void onSetup()
    {
        cam = Camera.main;
        instance = this;
        autoCooldown = true;
        pool = new ComponentPool<BlackHole>(prefabBlackHole);
        poolWhite = new ComponentPool<WhiteHole>(prefabWhiteHole);
        poolShockwaves = new ComponentPool<Shockwave>(prefabShockwave);
        poolExplosion = new ComponentPool<ParticleSystem>(explosion);
        poolExplosion.setTimer(3f);
        blackHoleDuration = stats.attackSpeed;
        blackHoleSize = stats.range;
        blackHoleForce = stats.knockback;
        blackHoleDps = stats.baseDamage.getRandom();
        doSpawnWhiteHole = fullStats.generic.boolA;
        doWormHole = fullStats.generic.boolB;
        doExplode = fullStats.generic.boolC;
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
    
    public static void SpawnShockwave(Vector2 position)
    {
        Debug.Log("Explode");
        Shockwave shockwave = instance.poolShockwaves.get(position);
        if (instance.doExplode)
        {
            shockwave.setPsPool(instance.poolExplosion);
        }
        shockwave
            .Setup(instance.blackHoleSize * 0.5f, shockwaveDamage, status.none, 0)
            .setPool(instance.poolShockwaves)
            .doShockwave(true);
    }
}
