using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesDropResource : Artefact
{
    private SingleStackerSingleThreshold stacker;

    public override void onSetup()
    {
        stacker = new SingleStackerSingleThreshold();
        stacker
            .setThreshold(stats.projectiles);
    }
    
}
