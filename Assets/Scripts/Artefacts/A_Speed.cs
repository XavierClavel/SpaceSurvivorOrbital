using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Speed : Artefact
{
    public override void Boost(BonusManager bonusManager)
    {
        BonusManager.current.addBonusSpeed(fullStats.generic.floatA);

    }

}
