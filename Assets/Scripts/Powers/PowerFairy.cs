using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <pre>
 * <p> BaseDamage -> Lightning strike damage </p>
 * <p> Cooldown -> Delay between lightning strikes </p>
 * <p> Magazine -> Amount of lightning strikes </p>
 * <p> BoolA -> Big fairy </p>
 * <p> BoolB -> Resurrection </p>
 * </pre>
 */
public class PowerFairy : Power, IPlayerEvents
{
    public Fairy fairy;
    [SerializeField] private Vector2 fairyOffset1;
    [SerializeField] private Vector2 fairyOffset2;
    private bool isResurrectionAvailable = false;
    
    public override void onSetup()
    {
        isResurrectionAvailable = fullStats.generic.boolA;
        if (isResurrectionAvailable) PlayerEventsManager.registerListener(this);
        
        Fairy newFairy = Instantiate(fairy);
        newFairy.Setup(stats, Vector2.zero,stats.pierce == 4);
        
        if (stats.magazine >= 1) Instantiate(fairy, transform.position, Quaternion.identity).Setup(stats, fairyOffset1);
        if (stats.magazine == 2) Instantiate(fairy, transform.position, Quaternion.identity).Setup(stats, fairyOffset2);
    }

    public bool onPlayerDeath()
    {
        if (!isResurrectionAvailable) return false;
        isResurrectionAvailable = false;
        
        PlayerController.Heal(1);
        return true;
    }

    private void OnDestroy()
    {
        PlayerEventsManager.unregisterListener(this);
    }
}
