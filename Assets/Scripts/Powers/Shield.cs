using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equipment
{
    
    public override void Boost(BonusManager bonusManager)
    {
        int shieldsAmount = stats.projectiles;
        Debug.Log(shieldsAmount);
        if (fullStats.generic.boolA && PlayerController.instance != null) PlayerController.instance.reflectsProjectiles = true;
        bonusManager.addBonusShield(shieldsAmount);
    }
}
