using UnityEngine;

public class Suit: Equipment
{
    protected override void onUse()
    {
        int additionalMaxHearts = fullStats.character.maxHealth;
        Debug.Log(additionalMaxHearts);
        PlayerController.bonusManager.addBonusMaxHealth(additionalMaxHearts);
    }
}
