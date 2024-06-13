using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : Equipment
{
    public override void Boost(BonusManager bonusManager)
    {
        PlayerController.instance.dashAvailable = true;
        PlayerController.instance.maxDashes = stats.projectiles;

        PlayerController.instance.dashCooldown = stats.cooldown;

        BonusManager.current.addBonusSpeed(fullStats.generic.floatA);
    }

}
