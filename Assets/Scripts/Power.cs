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
        playerTransform = PlayerController.instance.transform;
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
    
    public virtual void onSetup() {}

    protected override void onUse()
    {

    }

}
