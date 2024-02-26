using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Heart : Artefact
{
    public override void Boost(BonusManager bonusManager)
    {
        BonusManager.current.addBonusMaxHealth(stats.projectiles);

    }

}
