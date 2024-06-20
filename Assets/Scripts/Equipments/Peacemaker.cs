using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peacemaker : Equipment, IPlayerEvents
{
    [SerializeField] ParticleSystem revivalPS;
    private int addResurrection;
    private int currentResurrection;
    public override void onSetup()
    {
        addResurrection = stats.magazine;
        currentResurrection = 0 + addResurrection;
        Debug.Log("Resurrection :" + currentResurrection);
        EventManagers.player.registerListener(this);
    }
    public bool onPlayerDeath()
    {
        if (currentResurrection == 0) return false;

        Instantiate(revivalPS, playerTransform.position, Quaternion.identity).Play();

        PlayerController.Heal(PlayerController.instance.maxHealth);
        currentResurrection--;
        return true;
    }
    public bool onPlayerHit(bool shieldHit)
    {
        return false;
    }
    private void OnDestroy()
    {
        EventManagers.player.unregisterListener(this);
    }
}
