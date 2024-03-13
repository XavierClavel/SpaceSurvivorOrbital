using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_MysticCooldown : Artefact
{
    public override void Boost(BonusManager bonusManager)
    {
        BonusManager.current.addBonusPowerCooldown(fullStats.generic.floatA);
    }

}
