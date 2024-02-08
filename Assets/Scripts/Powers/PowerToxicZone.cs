using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <pre>
 * <p> BaseDamage -> Toxic Zone Dps </p>
 * <p> Cooldown -> Delay between toxic zones </p>
 * <p> Projectiles -> Amount of toxic zones </p>
 * <p> BoolA -> Increase player speed </p>
 * <p> BoolB -> Increase player damage </p>
 * <p> BoolC -> Decrease ennemy speed </p>
 * <p> BoolD -> Heal on kill </p>
 * <p> Projectiles -> Amount of toxic zones </p>
 * <p> Range -> Scale of toxic zones </p>
 * <p> BaseSpeed -> Speed towards player </p>
 * </pre>
 */
public class PowerToxicZone : Power, IEnnemyListener
{

    ComponentPool<ToxicZone> pool;
    [SerializeField] private ToxicZone toxicZonePrefab;
    private static PowerToxicZone instance;

    //dict ennemy -> how many toxic zones he is in
    private static Stacker<Ennemy> ennemyStacker;
    private static SingleStacker playerStacker;
    private static SingleStacker killStacker;

    private Camera cam;
    private bool doFreezeEnnemies;
    private bool doIncreasePlayerSpeed;
    private bool doIncreasePlayerDamage;
    private bool doHealOnKill;
    private float toxicZoneSpeed;
    
    public override void onSetup()
    {
        autoCooldown = true;
        instance = this;

        toxicZoneSpeed = stats.attackSpeed;
        doIncreasePlayerSpeed = fullStats.generic.boolA;
        doIncreasePlayerDamage = fullStats.generic.boolB;
        doFreezeEnnemies = fullStats.generic.boolC;
        doHealOnKill = fullStats.generic.boolD;

        if (doFreezeEnnemies)
        {
            StartCoroutine(nameof(ReapplyEffect));
        }

        pool = new ComponentPool<ToxicZone>(toxicZonePrefab);
        cam = Camera.main;

        ennemyStacker = new Stacker<Ennemy>();

        if (doFreezeEnnemies)
        {
            ennemyStacker.addOnStartStackingEvent(FreezeEnnemy);
        }    

        playerStacker = new SingleStacker();
        if (doIncreasePlayerSpeed)
        {
            playerStacker
                .addOnStartStackingEvent(PlayerController.ApplySpeedBoost)
                .addOnStopStackingEvent(PlayerController.RemoveSpeedBoost);
        }

        if (doIncreasePlayerDamage)
        {
            playerStacker
                .addOnStartStackingEvent(PlayerController.ApplyStrengthBoost)
                .addOnStopStackingEvent(PlayerController.RemoveStrengthBoost);
        }

        killStacker = new SingleStacker();
        if (doHealOnKill)
        {
            killStacker.addThresholdAction(10, delegate
            {
                PlayerController.Heal();
                killStacker.reset();
            });
            playerStacker.addOnStopStackingEvent(killStacker.reset);
        }
        
        Ennemy.registerListener(this);
    }
    
    private void FixedUpdate()
    {
        DealDamage();
    }

    private IEnumerator ReapplyEffect()
    {
        while (true)
        {
            yield return Helpers.getWait(1f);
            foreach (var ennemy in ennemyStacker.get())
            {
                ennemy.ApplyEffect(status.ice);
            }
        }
    }
    
    /**
     * Registers that a specific ennemy has entered a toxic zone, and updates the dictionary.
     */
    public static void onEnnemyEnterToxicZone(GameObject go)
    {
        if (!ObjectManager.dictObjectToEnnemy.ContainsKey(go))
        {
            return;
        }
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[go];
        ennemyStacker.stack(ennemy);
    }
    
    private void FreezeEnnemy(Ennemy ennemy) => ennemy.ApplyEffect(status.ice);
    
    /**
     * Registers that a specific ennemy has exited a toxic zone, and updates the dictionary.
     */
    public static void onEnnemyExitToxicZone(GameObject go)
    {
        if (!ObjectManager.dictObjectToEnnemy.ContainsKey(go))
        {
            return;
        }
        Ennemy ennemy = ObjectManager.dictObjectToEnnemy[go];
        ennemyStacker.unstack(ennemy);
    }

    public static void onPlayerEnterToxicZone()
    {
        playerStacker.stack();
        Debug.Log(playerStacker.get());
    }
    public static void onPlayerExitToxicZone()
    {
        playerStacker.unstack();
        Debug.Log(playerStacker.get());
    }
    
    /**
     * Runs through every ennemy currently inside a toxic zone, using the dictionary, and stacks damage to it.
     */
    private void DealDamage()
    {
        foreach (Ennemy ennemy in ennemyStacker.get())
        {
            ennemy.StackDamage(stats.baseDamage.x, new HashSet<status>());
        }
    }
    
    protected override void onUse()
    {
        StartCoroutine(nameof(SpawnToxicZones));
    }
    
    /**
     * Spawns the required amounts of toxic zones inside the bounds of the camera with a small delay between each.
     */
    IEnumerator SpawnToxicZones()
    {
        Bounds cameraBounds = cam.getBounds();
        for (int i = 0; i < stats.projectiles; i++)
        {
            Spawn(cameraBounds);
            yield return Helpers.getWait(0.2f);
        }
    }
    
    /**
     * Spawns a toxic zone inside the given bounds.
     * <param name="bounds">The bounds of the camera.</param>
     */
    private void Spawn(Bounds bounds)
    {
        ToxicZone toxicZone = pool.get(bounds);
        toxicZone.Setup(stats.range, toxicZoneSpeed);
    }

    public static void recall(ToxicZone toxicZone)
    {
        instance.pool.recall(toxicZone);
    }

    public void onEnnemyDeath(Ennemy ennemy)
    {
        ennemyStacker.remove(ennemy);
        if (playerStacker.get() == 0)
        {
            return;
        }
        killStacker.stack();
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }
}
