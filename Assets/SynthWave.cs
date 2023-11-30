using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthWave : Power
{
    [SerializeField] private Shockwave shockwave;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;

    public override void Setup(PlayerData _)
    {
        base.Setup(_)
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;

        InvokeRepeating("DoShockwave", 0f, 2f);
    }

    protected override void Start()
    {
        
    }

    private void DoShockwave()
    {
        Debug.Log(player == null);
        Debug.Log(shockwave == null);

        Shockwave newShockwave = Instantiate(shockwave, player.transform, true);
        newShockwave.transform.localScale = Vector3.zero;
        newShockwave.transform.localPosition = Vector3.zero;
        newShockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
        newShockwave.doShockwave();
    }
}
