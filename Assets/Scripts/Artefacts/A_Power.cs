using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class A_Power : Artefact
{
    
    public override void Boost(BonusManager bonusManager)
    {
        PlayerController.bonusManager.addBonusStrength(stats.baseDamage.x);
    }

}
