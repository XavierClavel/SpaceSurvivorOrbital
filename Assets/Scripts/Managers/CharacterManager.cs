using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterManager
{
    private const string characterBase = "0";
    private const string characterMaxHearts = "1";
    private const string characterShield = "2";
    private const string characterSpeed = "3";
    private const string characterStrength = "4";



    public static void applyCharacterEffect(characterStats stats, string character)
    {
        switch (character)
        {
            case characterBase:
                return;
            
            case characterMaxHearts:
                stats.maxHealth = 4;
                return;
            
            case characterSpeed:
                stats.baseSpeed = 5.5f;
                return;
            
            case characterStrength:
                stats.damageMultiplier = 1.1f;
                return;
            
            case characterShield:
                stats.maxShields = 2;
                stats.maxHealth = 0;
                return;
            
            default :
                Debug.LogWarning($"Character {character} has no effect implemented yet");
                return;
        }

        ;
    }
    
}
