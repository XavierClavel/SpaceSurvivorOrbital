using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Power{}
public class Power : Damager
{
    [SerializeField] protected bool activateOnStart;
    protected Transform playerTransform;

    public override void Setup(PlayerData stats)
    {
        try
        {
            playerTransform = PlayerController.instance.transform;
        }
        catch (Exception)
        {
            
        }
        isUsing = true;
        base.Setup(stats);

        if (activateOnStart)
        {
            Use();
        }
        else
        {
            StartCoroutine(nameof(Cooldown));
        }
        onSetup();
    }
    
    public virtual void Boost(BonusManager bonusManager) {}
    
    public virtual void onSetup() {}

    protected override void onUse()
    {

    }

}
