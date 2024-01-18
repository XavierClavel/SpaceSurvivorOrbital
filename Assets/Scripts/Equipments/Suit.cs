using UnityEngine;

public class Suit: Equipment
{
    protected override void onUse()
    {
        int additionalMaxHearts = fullStats.character.maxHealth;
        PlayerController.bonusManager.addBonusMaxHealth(additionalMaxHearts);
    }
}
