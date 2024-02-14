using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterManager
{
    private const string characterBase = "Knil";
    private const string characterMaxHearts = "1";
    private const string characterShield = "2";
    private const string characterSpeed = "Tifart";
    private const string characterStrength = "4";
    private const string characterResources = "5";
    private const string characterRadar = "6";
    private const string characterAltar = "7";

    public static void applyCharacterEffect(this BonusManager bonusManager) =>
        applyCharacterEffect(bonusManager, PlayerManager.character.getKey());

    public static void applyCharacterEffect(this BonusManager bonusManager, string character)
    {
        switch (character)
        {
            case characterBase:
                return;
            
            case characterMaxHearts:
                bonusManager.addBonusMaxHealth(2);
                return;
            
            case characterSpeed:
                bonusManager.addBonusSpeed(1.3f);
                return;
            
            case characterStrength:
                bonusManager.addBonusStrength(1.25f);
                return;
            
            case characterShield:
                bonusManager.addBonusMaxHealth(-3);
                bonusManager.addBonusShield(2);
                return;
            
            case characterResources:
                bonusManager.addBonusResources(2f);
                return;
            
            case characterAltar:
                bonusManager.addBonusAltarItem(1);
                return;
            
            default :
                Debug.LogWarning($"Character {character} has no effect implemented yet");
                return;
        }

        ;
    }
    
}
