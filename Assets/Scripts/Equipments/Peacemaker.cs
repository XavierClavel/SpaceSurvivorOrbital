using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peacemaker : Equipment, IPlayerEvents
{
    [SerializeField] ParticleSystem revivalPS;
    private int currentResurrection;
    private bool resurrectionUsed = false;
    public override void onSetup()
    {
        ResurrectionManager.peaceMaker.setMax(stats.magazine);
        //PlayerManager.resurrection += addResurrection;
        Debug.Log("Resurrection :" + currentResurrection);
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
