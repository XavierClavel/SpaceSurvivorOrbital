using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Power
{
    protected override void onUse()
    {
        int shieldsAmount = stats.projectiles;
        PlayerController.instance.SetupShields(shieldsAmount);
    }
}
