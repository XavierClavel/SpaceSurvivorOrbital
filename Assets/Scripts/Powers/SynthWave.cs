using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthWave : Power
{
    [SerializeField] private Shockwave shockwave;

    private bool isShockwaveEnabled;
    private bool doubleShockwave;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;

    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
        doubleShockwave = _.generic.boolB;
        
        
        shockwave = Instantiate(shockwave, player.transform, true);
        shockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);

        InvokeRepeating(nameof(DoShockwave), 0f, stats.attackSpeed);
        if (doubleShockwave) {InvokeRepeating(nameof(DoShockwave), 0.5f, stats.attackSpeed); }
    }


    private void DoShockwave()
    {
        shockwave.transform.localScale = Vector3.zero;
        shockwave.transform.localPosition = Vector3.zero;
        shockwave.doShockwave();
    }
}
