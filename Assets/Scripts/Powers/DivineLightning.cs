using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

/**
 * <pre>
 * <p> BaseDamage -> Lightning strike damage </p>
 * <p> Cooldown -> Delay between lightning strikes </p>
 * <p> Projectiles -> Amount of lightning strikes </p>
 * <p> BoolB -> Whether electric zones spawn </p>
 * <p> floatA -> Scale of the electric zone </p>
 * </pre>
 */
public class DivineLightning : Power, IElecZone
{
    public Counter elecZoneCounter;
    static readonly Vector2 range = new Vector2(14f, 8f);
    LayerMask mask;
    [SerializeField] ParticleSystem lightningStrike;
    [SerializeField] ParticleSystem lightningStrikeBig;

    ComponentPool<ParticleSystem> pool;
    [SerializeField] private ElectricZone electricZonePrefab;
    private int amountElecZoneTouched;
    public static DivineLightning instance;
    private bool stunChance;
    private bool doSpawnElecZone;
    private bool spawnElecZoneCounter = true;
    private float scaleElecZone;
    private bool bigLightning = false;
    private Camera cam;

    protected override void Start()
    {
        base.Start();
        instance = this;
        autoCooldown = true;
        mask = LayerMask.GetMask(Vault.layer.Ennemies, Vault.layer.Resources);
        bigLightning = fullStats.generic.boolC;

        if (bigLightning)
        {
            pool = new ComponentPool<ParticleSystem>(lightningStrikeBig).setTimer(1f);
        } else
        {
            pool = new ComponentPool<ParticleSystem>(lightningStrike).setTimer(1f);
        }
       
        doSpawnElecZone = fullStats.generic.boolB;
        stunChance = fullStats.generic.boolA;
        scaleElecZone = fullStats.generic.floatA;

        elecZoneCounter = new Counter(Orchestrator.context, fullStats.generic.floatB);
        elecZoneCounter.addOnStartEvent(ElecEventManager.ElecStart)
            .addOnCompleteEvent(ElecEventManager.ElecStop);
        
        ElecEventManager.registerListener(this);
        cam = Camera.main;
    }

    private void OnDestroy()
    {
        ElecEventManager.unregisterListener(this);
    }

    public void EnterElecZone()
    {
        elecZoneCounter.ResetCounter();
    }

    public void ExitElecZone()
    {
        elecZoneCounter.ResetCounter();
    }

    public void OnElecStart()
    {
        InteractorHandler.playerInteractorHandler.AddBonusStatus(status.lightning);
    }

    public void OnElecStop()
    {
        InteractorHandler.playerInteractorHandler.RemoveBonusStatus(status.lightning);
    }
    
    

    void SpawnEletricZone(Vector3 spawnPoint)
    {
        ElectricZone electricZone = Instantiate(electricZonePrefab);
        electricZone.transform.position = spawnPoint;
        electricZone.transform.localScale = Vector3.zero;
        electricZone.timerDuration = fullStats.generic.floatB;
        //TODO Fade Out and get smaller or just dispawn
        electricZone.Setup(scaleElecZone);
    }

    protected override void onUse()
    {
        Bounds cameraBounds = cam.getBounds();
        List<Collider2D> hits = Physics2D.OverlapAreaAll(cameraBounds.min, cameraBounds.max, mask).ToList();
        for (int i = 0; i < stats.projectiles; i++)
        {
            if (hits.Count == 0)
            {
                Strike(cameraBounds.getRandom());
                continue;
            }
            Strike(hits.popRandom().transform.position);
        }
    }

    private void Strike(Vector3 hitPoint)
    {
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(hitPoint, stats.range, mask);
        status effect = stats.element;
        SoundManager.PlaySfx(transform, key: "Lighning_Strike");
        ShakeManager.Shake(1f, 0.2f);
        if (stunChance)
        {
            if (Helpers.ProbabilisticBool(0.5f)) effect = status.lightning;
        }
        Hit(collidersInRadius, effect: effect);

        if (doSpawnElecZone)
        {
            if (spawnElecZoneCounter) SpawnEletricZone(hitPoint);
            spawnElecZoneCounter = !spawnElecZoneCounter;
        }

        ParticleSystem ps = pool.get(hitPoint);
        ps.Play();
    }
}