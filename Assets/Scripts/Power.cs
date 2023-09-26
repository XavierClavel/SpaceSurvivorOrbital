using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Power : Damager
{
    [SerializeField] private bool activateOnStart;
    
    protected override void Setup(interactorStats stats, bool dualUse = false) {
        base.Setup(stats, dualUse);

        if (activateOnStart) {
            Use();
        } else {
            StartCoroutine(nameof(Cooldown));
        }
    }

}
