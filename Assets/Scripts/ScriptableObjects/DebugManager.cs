using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
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
    [SerializeField] private bool noMapGeneration;
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
    [SerializeField] private bool flameThrower;
    [SerializeField] private bool iceSpike;
    [SerializeField] private bool huntersMark;

    [Header("Start with equipments")] 
    [SerializeField] private bool shield;
    [SerializeField] private bool suit;
    [SerializeField] private bool stock;
    [SerializeField] private bool radar;
    [SerializeField] private bool jetpack;
    [SerializeField] private bool peacemaker;

    [Header("Start with artefacts")]
    [SerializeField] private bool ennemiesDropResources;
    [SerializeField] private bool Heart;
    [SerializeField] private bool mysticCooldown1;
    [SerializeField] private bool mysticCooldown2;
    [SerializeField] private bool mysticCooldown3;
    [SerializeField] private bool mysticPower1;
    [SerializeField] private bool mysticPower2;
    [SerializeField] private bool mysticPower3;
    [SerializeField] private bool power1;
    [SerializeField] private bool power2;
    [SerializeField] private bool power3;
    [SerializeField] private bool speed1;
    [SerializeField] private bool speed2;
    [SerializeField] private bool speed3;


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

    public static bool doNoMapGeneration()
    {
        return instance.debugEnabled && instance.noMapGeneration;
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
        if (flameThrower) AcquirePower(Vault.power.FlameThrower);
        if (iceSpike) AcquirePower(Vault.power.IceSpike);
        if (huntersMark) AcquirePower(Vault.power.HuntersMark);

        //Equipments
        if (shield) AcquireEquipment(Vault.power.Shield);
        if (suit) AcquireEquipment("Suit");
        if (stock) AcquireEquipment("Stock");
        if (radar) AcquireEquipment("Radar");
        if (jetpack) AcquireEquipment("Jetpack");
        if (peacemaker) AcquireEquipment("Peacemaker");

        //Artefacts
        if (ennemiesDropResources) AcquireArtefact("Dropper");
        if (Heart) AcquireArtefact("Heart");
        if (mysticCooldown1) AcquireArtefact("MysticCooldown1");
        if (mysticCooldown2) AcquireArtefact("MysticCooldown2");
        if (mysticCooldown3) AcquireArtefact("MysticCooldown3");
        if (mysticPower1) AcquireArtefact("MysticPower1");
        if (mysticPower2) AcquireArtefact("MysticPower2");
        if (mysticPower3) AcquireArtefact("MysticPower3");
        if (power1) AcquireArtefact("Power1");
        if (power2) AcquireArtefact("Power2");
        if (power3) AcquireArtefact("Power3");
        if (speed1) AcquireArtefact("Speed1");
        if (speed2) AcquireArtefact("Speed2");
        if (speed3) AcquireArtefact("Speed3");


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
        ScriptableObjectManager.dictKeyToEquipmentHandler.Keys.ForEach(it => Debug.Log(it));
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
