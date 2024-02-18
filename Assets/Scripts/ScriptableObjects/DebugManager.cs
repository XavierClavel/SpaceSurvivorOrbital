using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DebugManager", menuName = Vault.other.scriptableObjectMenu + "DebugManager", order = 0)]
public class DebugManager : ScriptableObject
{
    [SerializeField] private bool debugEnabled = true;
    
    [Header("Game Rules")]
    [SerializeField] private bool noEnnemySpawn;
    [SerializeField] private bool noTimer;
    [SerializeField] private bool startWithResources;
    [SerializeField] private bool freeUpgrades;
    [SerializeField] private bool shipPresent;
    [SerializeField] private bool shipInstantTP;

    [Header("Start with powers")] 
    [SerializeField] private bool divineLightning;
    [SerializeField] private bool fairy;
    [SerializeField] private bool ghost;
    [SerializeField] private bool blackHole;
    [SerializeField] private bool synthWave;
    [SerializeField] private bool toxicZone;
    [SerializeField] private bool dagger;

    [Header("Start with equipments")] 
    [SerializeField] private bool shield;
    [SerializeField] private bool suit;
    [SerializeField] private bool stock;
    [SerializeField] private bool radar;

    [Header("Start with artefacts")] [SerializeField]
    private bool ennemiesDropResources;

    [Header("Others")] 
    [SerializeField] private bool spawnBossOnStart;

    [SerializeField] private bool overrideDifficulty;
    [SerializeField] private int debugDifficulty;
    [SerializeField] private bool overrideProgressionUnlocks;
    
    private RectTransform debugLayout;
    private TextMeshProUGUI debugLine;

    public static DebugManager instance;

    public static bool areUpgradesFree()
    {
        return instance.debugEnabled && instance.freeUpgrades;
    }

    public static bool isShipPresent()
    {
        return instance.debugEnabled && instance.shipPresent;
    }

    public static bool isShipTpInstant()
    {
        return instance.debugEnabled && instance.shipInstantTP;
    }

    public static bool doNoEnnemySpawn()
    {
        return instance.debugEnabled && instance.noEnnemySpawn;
    }

    public static bool doNoTimer()
    {
        return instance.debugEnabled && instance.noTimer;
    }

    public static bool doSpawnBossOnStart()
    {
        return instance.debugEnabled && instance.spawnBossOnStart;
    }

    public static bool doOverrideDifficulty()
    {
        return instance.debugEnabled && instance.overrideDifficulty;
    }

    public static int getDebugDifficulty()
    {
        return instance.debugDifficulty;
    }

    public static bool doOverrideProgressionUnlocks() => instance.debugEnabled && instance.overrideProgressionUnlocks;


    public void LoadData()
    {
        instance = this;
        if (!debugEnabled) return;
        
        //Powers
        if (divineLightning) AcquirePower(Vault.power.DivineLightning);
        if (fairy) AcquirePower(Vault.power.Fairy);
        if (ghost) AcquirePower(Vault.power.Ghost);
        if (blackHole) AcquirePower(Vault.power.BlackHole);
        if (synthWave) AcquirePower(Vault.power.SynthWave);
        if (toxicZone) AcquirePower(Vault.power.ToxicZone);
        if (dagger) AcquirePower(Vault.power.Dagger);
        
        //Equipments
        if (shield) AcquireEquipment(Vault.power.Shield);
        if (suit) AcquireEquipment("Suit");
        if (stock) AcquireEquipment("Stock");
        if (radar) AcquireEquipment("Radar");
        
        //Artefacts
        if (ennemiesDropResources) AcquireArtefact("First");

    }

    public static void DisplayValue(string name, string value)
    {
        return;
        if (instance.debugLayout == null) return;
        var obj = GameObject.Instantiate(instance.debugLine);
        obj.transform.SetParent(instance.debugLayout);
        obj.SetText($"{name}: {value}");
    }
    
    static void AcquirePower(string key)
    {
        PowerHandler powerHandler = ScriptableObjectManager.dictKeyToPowerHandler[key];
        if (PlayerManager.powers.Contains(powerHandler)) return;
        PlayerManager.AcquirePower(powerHandler);
    }
    
    static void AcquireEquipment(string key)
    {
        EquipmentHandler equipmentHandler = ScriptableObjectManager.dictKeyToEquipmentHandler[key];
        if (PlayerManager.equipments.Contains(equipmentHandler)) return;
        PlayerManager.AcquireEquipment(equipmentHandler);
    }
    
    static void AcquireArtefact(string key)
    {
        ArtefactHandler artefactHandler = ScriptableObjectManager.dictKeyToArtefactHandler[key];
        if (PlayerManager.artefacts.Contains(artefactHandler))
        {
            Debug.Log($"Key {key} not found");
            return;
        }
        PlayerManager.AcquireArtefact(artefactHandler);
    }

}
