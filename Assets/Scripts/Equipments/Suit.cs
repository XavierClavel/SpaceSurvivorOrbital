using UnityEngine;

public class Suit: Equipment
{
    public override void onSetup()
    {
        int additionalMaxHearts = stats.projectiles;
        PlayerController.bonusManager.addBonusMaxHealth(additionalMaxHearts);
    }
}
