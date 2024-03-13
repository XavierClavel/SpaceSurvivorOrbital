using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : Equipment
{
    public override void Boost(BonusManager bonusManager)
    {
        int additionalResourceAmount = stats.projectiles;
        BonusManager.current.addBonusStock(additionalResourceAmount);
    }
}
