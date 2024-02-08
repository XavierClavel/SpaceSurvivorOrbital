using UnityEngine;

public class Suit: Equipment
{
    public override void Boost(BonusManager bonusManager)
    {
        int additionalMaxHearts = stats.projectiles;
        PlayerController.bonusManager.addBonusMaxHealth(additionalMaxHearts);
    }
}
