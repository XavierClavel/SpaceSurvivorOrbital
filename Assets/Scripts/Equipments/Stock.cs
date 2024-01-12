using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : Equipment
{
    protected override void onUse()
    {
        int additionalResourceAmount = stats.projectiles;
        PlayerController.instance.AddBonusStock(additionalResourceAmount);
    }
}
