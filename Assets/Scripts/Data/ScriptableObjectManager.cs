using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ScriptableObjectManager
{
    public static Dictionary<string, ButtonSprite> dictKeyToButtonSprites;
    public static Dictionary<string, PowerHandler> dictKeyToPowerHandler;
    public static Dictionary<string, WeaponHandler> dictKeyToWeaponHandler;
    public static Dictionary<string, CharacterHandler> dictKeyToCharacterHandler;
    static List<WeaponHandler> baseWeapons;
    static List<CharacterHandler> characters;

    public static List<CharacterHandler> getCharacters()
    {
        return characters;
    }


    public static List<WeaponHandler> getWeapons()
    {
        return baseWeapons;
    }

    public static void LoadScriptableObjects()
    {
        dictKeyToButtonSprites = new Dictionary<string, ButtonSprite>();
        ButtonSprite[] buttonSprites = Resources.LoadAll<ButtonSprite>("ButtonSprites/");
        foreach (ButtonSprite buttonSprite in buttonSprites)
        {
            dictKeyToButtonSprites[buttonSprite.key] = buttonSprite;
        }

        dictKeyToPowerHandler = new Dictionary<string, PowerHandler>();
        PowerHandler[] powerHandlers = Resources.LoadAll<PowerHandler>(Vault.path.Powers);
        foreach (PowerHandler powerHandler in powerHandlers)
        {
            dictKeyToPowerHandler[powerHandler.getKey()] = powerHandler;
        }

        dictKeyToWeaponHandler = new Dictionary<string, WeaponHandler>();
        baseWeapons = new List<WeaponHandler>();
        WeaponHandler[] weaponHandlers = Resources.LoadAll<WeaponHandler>(Vault.path.BaseWeapons);
        foreach (WeaponHandler weaponHandler in weaponHandlers)
        {
            dictKeyToWeaponHandler[weaponHandler.getKey()] = weaponHandler;
            baseWeapons.Add(weaponHandler);
        }
        weaponHandlers = Resources.LoadAll<WeaponHandler>(Vault.path.AllWeapons);
        foreach (WeaponHandler weaponHandler in weaponHandlers)
        {
            dictKeyToWeaponHandler[weaponHandler.getKey()] = weaponHandler;
        }

        dictKeyToCharacterHandler = new Dictionary<string, CharacterHandler>();
        characters = new List<CharacterHandler>();
        CharacterHandler[] characterHandlers = Resources.LoadAll<CharacterHandler>(Vault.path.Characters);
        foreach (CharacterHandler characterHandler in characterHandlers)
        {
            dictKeyToCharacterHandler[characterHandler.getKey()] = characterHandler;
            characters.Add(characterHandler);
        }
    }
}
