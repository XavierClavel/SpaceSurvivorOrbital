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
 * <p> BoolD -> Move towards player </p>
 * <p> Projectiles -> Amount of toxic zones </p>
 * </pre>
 */
public class PowerToxicZone : Power
{
    static readonly Vector2 range = new Vector2(14f, 8f);

    ComponentPool<ToxicZone> pool;
    [SerializeField] private ToxicZone toxicZonePrefab;
    private static PowerToxicZone instance;
    
    //dict ennemy -> how many toxic zones he is in
    private Dictionary<GameObject, int> dictEnnemyToPresence = new Dictionary<GameObject, int>();

    private bool doFollowPlayer = false;
    private Camera cam;
    
    protected override void Start()
    {
        base.Start();
        autoCooldown = true;
        instance = this;

        pool = new ComponentPool<ToxicZone>(toxicZonePrefab);
        cam = Camera.main;
    }
    
    public static void OnEnnemyEnterToxicZone(GameObject ennemy)
    {
        if (instance.dictEnnemyToPresence.ContainsKey(ennemy)) instance.dictEnnemyToPresence[ennemy]++;
        else instance.dictEnnemyToPresence[ennemy] = 1;
    }

    public static void OnEnnemyExitToxicZone(GameObject ennemy)
    {
        if (!instance.dictEnnemyToPresence.ContainsKey(ennemy)) return;
        instance.dictEnnemyToPresence[ennemy]--;
        if (instance.dictEnnemyToPresence[ennemy] == 0) instance.dictEnnemyToPresence.Remove(ennemy);
    }

    private void FixedUpdate()
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

    IEnumerator SpawnToxicZones()
    {
        Bounds cameraBounds = cam.getBounds();
        for (int i = 0; i < stats.projectiles; i++)
        {
            Spawn(cameraBounds);
            yield return Helpers.GetWait(0.2f);
        }
    }

    private void Spawn(Bounds bounds)
    {
        ToxicZone toxicZone = pool.get(bounds);
        toxicZone.Setup(stats.range, doFollowPlayer);
    }

    public static void recall(ToxicZone toxicZone)
    {
        instance.pool.recall(toxicZone);
    }
}
