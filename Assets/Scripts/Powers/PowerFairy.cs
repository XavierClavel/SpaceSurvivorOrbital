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
    [SerializeField] private Vector3 fairyOffset1;
    [SerializeField] private Vector3 fairyOffset2;
    [SerializeField] ParticleSystem revivalPS;
    private bool isResurrectionAvailable = false;
    
    public override void onSetup()
    {
        isResurrectionAvailable = fullStats.generic.boolB;
        if (isResurrectionAvailable) EventManagers.player.registerListener(this);
        
        Fairy newFairy = Instantiate(fairy);
        newFairy.Setup(stats, Vector2.zero,fullStats.generic.boolA);
        
        if (stats.magazine >= 1) Instantiate(fairy, transform.position + fairyOffset1, Quaternion.identity).Setup(stats, transform.position + fairyOffset1);
        if (stats.magazine == 2) Instantiate(fairy, transform.position + fairyOffset2, Quaternion.identity).Setup(stats, transform.position + fairyOffset2);
    }

    public bool onPlayerDeath()
    {
        if (!isResurrectionAvailable) return false;
        isResurrectionAvailable = false;
        
        Debug.Log("Resurrection");
        Instantiate(revivalPS, playerTransform.position, Quaternion.identity).Play();
        
        PlayerController.Heal(PlayerController.instance.maxHealth);
        return true;
    }

    public bool onPlayerHit(bool shieldHit)
    {
        return false;
    }

    public void onResourcePickup(resourceType type)
    {
        
    }

    private void OnDestroy()
    {
        EventManagers.player.unregisterListener(this);
    }
}
