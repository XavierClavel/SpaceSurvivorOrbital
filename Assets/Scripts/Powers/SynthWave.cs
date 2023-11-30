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

        InvokeRepeating("DoShockwave", 0f, stats.attackSpeed);
        if (doubleShockwave) {InvokeRepeating("DoShockwave", 0.5f, stats.attackSpeed); }
    }

    protected override void Start()
    {
        
    }

    private void DoShockwave()
    {
        Shockwave newShockwave = Instantiate(shockwave, player.transform, true);
        newShockwave.transform.localScale = Vector3.zero;
        newShockwave.transform.localPosition = Vector3.zero;
        newShockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
        newShockwave.doShockwave();
    }
}
