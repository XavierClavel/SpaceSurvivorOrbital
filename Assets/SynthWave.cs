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
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
    }

    protected override void Start()
    {
        InvokeRepeating("DoShockwave", 0f, 2f);
    }

    private void DoShockwave()
    {
        Debug.Log("La fonction a été appelée !");
        shockwave = Instantiate(shockwave, player.transform, true);
        shockwave.transform.localScale = Vector3.zero;
        shockwave.transform.localPosition = Vector3.zero;
        shockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
    }
}
