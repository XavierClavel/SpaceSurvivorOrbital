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
        
        stats.interactor.cooldown *= BonusManager.current.getPowerCooldownMultiplier();
        stats.interactor.baseDamage = new Vector2Int(
            (int)(stats.interactor.baseDamage.x * BonusManager.current.getPowerDamageMultiplier()),
            (int)(stats.interactor.baseDamage.y * BonusManager.current.getPowerDamageMultiplier())
            );
        base.Setup(stats);

        onSetup();

        if (activateOnStart)
        {
            Use();
        }
        else
        {
            StartCoroutine(nameof(Cooldown));
        }
    }
    
    public virtual void Boost(BonusManager bonusManager) {}
    
    public virtual void onSetup() {}

    protected override void onUse()
    {

    }

    public static int getDamage(Vector2Int baseDamage) =>
        (int)(baseDamage.getRandom() * BonusManager.current.getPowerDamageMultiplier());

}
