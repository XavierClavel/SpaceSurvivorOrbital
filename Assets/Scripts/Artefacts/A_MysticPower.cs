using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_MysticPower : Artefact
{
    public override void Boost(BonusManager bonusManager)
    {
        BonusManager.current.addPowerDamageMultiplier(fullStats.generic.floatA);
    }

}
