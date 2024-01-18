using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equipment
{
    
    protected override void onUse()
    {
        int shieldsAmount = stats.projectiles;
        if (fullStats.generic.boolA) PlayerController.instance.reflectsProjectiles = true;
        PlayerController.instance.SetupShields(shieldsAmount);
    }
}
