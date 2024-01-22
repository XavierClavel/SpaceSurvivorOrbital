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
 * <p> Projectiles -> Amount of toxic zones </p>
 * <p> Range -> Scale of toxic zones </p>
 * <p> BaseSpeed -> Speed towards player </p>
 * </pre>
 */
public class PowerToxicZone : Power
{

    ComponentPool<ToxicZone> pool;
    [SerializeField] private ToxicZone toxicZonePrefab;
    private static PowerToxicZone instance;
    
    //dict ennemy -> how many toxic zones he is in
    private Dictionary<GameObject, int> dictEnnemyToPresence = new Dictionary<GameObject, int>();

    private Camera cam;
    
    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        instance = this;

        pool = new ComponentPool<ToxicZone>(toxicZonePrefab);
        cam = Camera.main;
    }
    
    private void FixedUpdate()
    {
        DealDamage();
    }
    
    /**
     * Registers that a specific ennemy has entered a toxic zone, and updates the dictionary.
     */
    public static void OnEnnemyEnterToxicZone(GameObject ennemy)
    {
        if (instance.dictEnnemyToPresence.ContainsKey(ennemy)) instance.dictEnnemyToPresence[ennemy]++;
        else instance.dictEnnemyToPresence[ennemy] = 1;
    }
    
    /**
     * Registers that a specific ennemy has exited a toxic zone, and updates the dictionary.
     */
    public static void OnEnnemyExitToxicZone(GameObject ennemy)
    {
        if (!instance.dictEnnemyToPresence.ContainsKey(ennemy)) return;
        instance.dictEnnemyToPresence[ennemy]--;
        if (instance.dictEnnemyToPresence[ennemy] == 0) instance.dictEnnemyToPresence.Remove(ennemy);
    }
    
    /**
     * Runs through every ennemy currently inside a toxic zone, using the dictionary, and stacks damage to it.
     */
    private void DealDamage()
    {
        foreach (GameObject ennemy in dictEnnemyToPresence.Keys)
        {
            if (!ObjectManager.dictObjectToEnnemy.ContainsKey(ennemy))
            {
                continue;
            }
            ObjectManager.dictObjectToEnnemy[ennemy].StackDamage(stats.baseDamage.x, new HashSet<status>());
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
        toxicZone.Setup(stats.range, fullStats.character.baseSpeed);
    }

    public static void recall(ToxicZone toxicZone)
    {
        instance.pool.recall(toxicZone);
    }
}
