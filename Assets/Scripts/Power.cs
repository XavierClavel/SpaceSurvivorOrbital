using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Power : Damager
{
    [SerializeField] private bool activateOnStart;
    protected Transform playerTransform;

    public override void Setup(interactorStats stats, bool dualUse = false)
    {
        playerTransform = PlayerController.instance.transform;
        isUsing = true;
        base.Setup(stats, dualUse);

        if (activateOnStart)
        {
            Use();
        }
        else
        {
            StartCoroutine(nameof(Cooldown));
        }
    }

    protected override void onUse()
    {

    }

}
