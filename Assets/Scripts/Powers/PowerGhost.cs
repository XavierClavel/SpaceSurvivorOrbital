using UnityEngine;

/**
 * <pre>
 * <p> IntA -> Shockwave damage </p>
 * <p> Float A -> Shockwave range </p>
 * <p> Element A -> Shockwave element </p>
 * <p> BoolA -> Big ghost </p>
 * <p> BoolB -> Explode on contact with ennemy </p>
 * <p> BoolC -> Explode on hit by player </p>
 * <p> BaseSpeed -> Speed used by ghost to follow ennemies </p>
 * <p> IntB -> Spawn rate (every x) </p>
 * <p> Projectiles -> Amount of bullets fired </p>
 * <p> Base Damage -> Projectiles damage </p>
 * <p> Attack Speed -> Projectiles speed </p>
 * </pre>
 */
public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private Ghost ghost;
    [SerializeField] private Ghost bigGhost;
    [SerializeField] private Shockwave ghostShockwave;
    [SerializeField] private Bullet projectile;

    private int spawnsEvery = 2;
    private readonly int bigGhostSpawnsEvery = 10;
    private int spawnCounter = 0;
    private int bigGhostSpawnCounter = 0;

    private ComponentPool<Ghost> poolGhosts;
    private ComponentPool<Ghost> poolBigGhosts;
    private ComponentPool<Shockwave> poolShockwaves;
    private ComponentPool<Bullet> poolBullets;
    
    private static PowerGhost instance;
    
    private static bool isBigGhostEnabled;
    private static int shockwaveDamage;
    private static float shockwaveMaxRange;
    private static status shockwaveElement;
    private int projectiles;

    private float projectilesDeltaAngle;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        isBigGhostEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
        spawnsEvery = _.generic.intB;
        projectiles = _.interactor.projectiles;
        
        EventManagers.ennemies.registerListener(this);
        poolGhosts = new ComponentPool<Ghost>(ghost);
        poolBigGhosts = new ComponentPool<Ghost>(bigGhost);
        poolShockwaves = new ComponentPool<Shockwave>(ghostShockwave);
        poolBullets = new ComponentPool<Bullet>(projectile);
        
        instance = this;

        if (projectiles > 0) projectilesDeltaAngle = 360f / (float)(projectiles - 1);
    }

    private void OnDestroy()
    {
        EventManagers.ennemies.unregisterListener(this);
    }

    public void onEnnemyDeath(Ennemy ennemy)
    {
        spawnCounter++;
        if (spawnCounter < spawnsEvery) return;
        spawnCounter = 0;
        SpawnGhost(ennemy.transform.position);
    }
    private void SpawnGhost(Vector2 position)
    {
        bigGhostSpawnCounter++;
        if (isBigGhostEnabled && bigGhostSpawnCounter >= bigGhostSpawnsEvery)
        {
            bigGhostSpawnCounter = 0;
            poolBigGhosts
                .get(position)
                .Setup(fullStats)
                .setBig(); 
            return;
        }
        Ghost newGhost = poolGhosts.get(position);
        newGhost.Setup(fullStats);
    }

    public static void recallGhost(Ghost ghostToRecall)
    {
        instance.poolGhosts.recall(ghostToRecall);
    }
    
    public static void SpawnShockwave(Vector2 position, bool bigGhost)
    {
        Shockwave shockwaveGhost = instance.poolShockwaves.get(position);
        shockwaveGhost.transform.localScale = Vector3.zero;
        if (bigGhost) shockwaveGhost.setup(2f * shockwaveMaxRange, 2 * shockwaveDamage, shockwaveElement, 0);
        else shockwaveGhost.setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement, 0);
        shockwaveGhost.setRecallMethod(delegate
        {
            PowerGhost.recallShockwave(shockwaveGhost);
            
        });
        shockwaveGhost.doShockwave(true);
    }

    public static void SpawnProjectiles(Vector2 position)
    {
        if (instance.projectiles == 0) return;
        for (int i = 0; i < instance.projectiles; i++)
        {
            instance.FireBullet(position, i * instance.projectilesDeltaAngle * Vector3.forward);
        }
        
    }
    
    void FireBullet(Vector3 position, Vector3 eulerRotation)
    {
        Bullet bullet = poolBullets.get(position, eulerRotation);
        HitInfo hitInfo = new HitInfo(stats.baseDamage.getRandom());
        bullet
            .setPool(poolBullets)
            .setTimer(1f)
            .Fire(stats.attackSpeed, hitInfo);
    }
    
    
    public static void recallShockwave(Shockwave shockwaveToRecall)
    {
        instance.poolShockwaves.recall(shockwaveToRecall);
    }

}
