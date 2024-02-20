using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Power : Artefact
{
    private Vector2 upDamage;

    public override void onSetup()
    {
        upDamage = stats.baseDamage;
    }
}
