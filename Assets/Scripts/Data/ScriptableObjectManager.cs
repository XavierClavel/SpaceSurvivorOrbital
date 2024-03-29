using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public static class ScriptableObjectManager
{
    public static Dictionary<string, ButtonSprite> dictKeyToButtonSprites;
    public static Dictionary<string, PowerHandler> dictKeyToPowerHandler;
    public static Dictionary<string, WeaponHandler> dictKeyToWeaponHandler;
    public static Dictionary<string, EquipmentHandler> dictKeyToEquipmentHandler;
    public static Dictionary<string, CharacterHandler> dictKeyToCharacterHandler;
    public static Dictionary<string, ArtefactHandler> dictKeyToArtefactHandler;
    public static Dictionary<planetType, TilesBank> dictTypeToTilesBank;
    public static Dictionary<string, BossData> dictKeyToBossData;
    public static Dictionary<string, Sfx> dictKeyToSfx;
    static List<WeaponHandler> baseWeapons;
    static List<CharacterHandler> characters;
    static List<EquipmentHandler> equipments;

    public static List<CharacterHandler> getCharacters() => characters;
    public static List<CharacterHandler> getDicoveredCharacters() => characters.Where(it => it.isDiscovered()).ToList();

    public static List<WeaponHandler> getWeapons() => baseWeapons;

    public static List<EquipmentHandler> getEquipments() => equipments;
    public static List<EquipmentHandler> GetDiscoveredEquipments() => equipments.Where(it => it.isDiscovered()).ToList();

    public static List<PowerHandler> getPowers() => dictKeyToPowerHandler.Values.ToList();
    public static List<PowerHandler> getDiscoveredPowers() => getPowers().Where(it => it.isDiscovered()).ToList();

    public static Sprite getIcon(string key)
    {
        if (dictKeyToWeaponHandler.ContainsKey(key)) return dictKeyToWeaponHandler[key].getIcon();
        if (dictKeyToEquipmentHandler.ContainsKey(key)) return dictKeyToEquipmentHandler[key].getIcon();
        if (dictKeyToPowerHandler.ContainsKey(key)) return dictKeyToPowerHandler[key].getIcon();
        Debug.LogError($"Scriptable object with key -{key}- not found");
        return null;
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
            baseWeapons.TryAdd(weaponHandler);
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
            characters.TryAdd(characterHandler);
        }

        dictKeyToEquipmentHandler = new Dictionary<string, EquipmentHandler>();
        equipments = new List<EquipmentHandler>();
        EquipmentHandler[] equipmentHandlers = Resources.LoadAll<EquipmentHandler>(Vault.path.Equipments);
        foreach (EquipmentHandler equipmentHandler in equipmentHandlers)
        {
            if (!equipmentHandler.canBeSelected()) continue;
            dictKeyToEquipmentHandler[equipmentHandler.getKey()] = equipmentHandler;
            equipments.TryAdd(equipmentHandler);
        }

        dictKeyToSfx = new Dictionary<string, Sfx>();
        Sfx[] sfxs = Resources.LoadAll<Sfx>("Sfx");
        foreach (Sfx sfx in sfxs)
        {
            dictKeyToSfx[sfx.getKey()] = sfx;
        }

        dictTypeToTilesBank = new Dictionary<planetType, TilesBank>();
        TilesBank[] tilesBanks = Resources.LoadAll<TilesBank>("TilesBanks");
        tilesBanks.ForEach(it => dictTypeToTilesBank[it.type] = it);

        dictKeyToBossData = new Dictionary<string, BossData>();
        BossData[] bossDatas = Resources.LoadAll<BossData>("BossData");
        bossDatas.ForEach(it => dictKeyToBossData[it.getKey()] = it);
    }
}
