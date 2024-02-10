using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ScriptableObjectManager
{
    public static Dictionary<string, ButtonSprite> dictKeyToButtonSprites;
    public static Dictionary<string, PowerHandler> dictKeyToPowerHandler;
    public static Dictionary<string, WeaponHandler> dictKeyToWeaponHandler;
    public static Dictionary<string, EquipmentHandler> dictKeyToEquipmentHandler;
    public static Dictionary<string, CharacterHandler> dictKeyToCharacterHandler;
    public static Dictionary<string, ArtefactHandler> dictKeyToArtefactHandler;
    public static Dictionary<string, Sfx> dictKeyToSfx;
    static List<WeaponHandler> baseWeapons;
    static List<CharacterHandler> characters;
    static List<EquipmentHandler> equipments;

    public static List<CharacterHandler> getCharacters()
    {
        return characters;
    }

    public static List<WeaponHandler> getWeapons()
    {
        return baseWeapons;
    }

    public static List<EquipmentHandler> getEquipment()
    {
        return equipments;
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
            if (!powerHandler.canBeSelected()) continue;
            dictKeyToPowerHandler[powerHandler.getKey()] = powerHandler;
        }
        
        dictKeyToArtefactHandler = new Dictionary<string, ArtefactHandler>();
        ArtefactHandler[] artefactHandlers = Resources.LoadAll<ArtefactHandler>(Vault.path.Artefacts);
        foreach (ArtefactHandler artefactHandler in artefactHandlers)
        {
            if (!artefactHandler.canBeSelected()) continue;
            dictKeyToArtefactHandler[artefactHandler.getKey()] = artefactHandler;
        }
        
        dictKeyToWeaponHandler = new Dictionary<string, WeaponHandler>();
        baseWeapons = new List<WeaponHandler>();
        WeaponHandler[] weaponHandlers = Resources.LoadAll<WeaponHandler>(Vault.path.BaseWeapons);
        foreach (WeaponHandler weaponHandler in weaponHandlers)
        {
            if (!weaponHandler.canBeSelected()) continue;
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
            if (!characterHandler.canBeSelected()) continue;
            dictKeyToCharacterHandler[characterHandler.getKey()] = characterHandler;
            characters.Add(characterHandler);
        }

        dictKeyToEquipmentHandler = new Dictionary<string, EquipmentHandler>();
        equipments = new List<EquipmentHandler>();
        EquipmentHandler[] equipmentHandlers = Resources.LoadAll<EquipmentHandler>(Vault.path.Equipments);
        foreach (EquipmentHandler equipmentHandler in equipmentHandlers)
        {
            if (!equipmentHandler.canBeSelected()) continue;
            dictKeyToEquipmentHandler[equipmentHandler.getKey()] = equipmentHandler;
            equipments.Add(equipmentHandler);
        }

        dictKeyToSfx = new Dictionary<string, Sfx>();
        Sfx[] sfxs = Resources.LoadAll<Sfx>("Sfx");
        foreach (Sfx sfx in sfxs)
        {
            dictKeyToSfx[sfx.getKey()] = sfx;
        }
    }
}
