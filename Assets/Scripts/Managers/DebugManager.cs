using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugManager : MonoBehaviour
{
    public static bool areUpgradesFree { get; private set; }
    
    
    public bool noEnnemySpawn;
    public bool noTimer;
    [SerializeField] bool startWithResources;
    [SerializeField] private bool freeUpgrades;
    public bool shipPresent;
    public bool shipInstantTP;

    [Header("Radar")] 
    public bool displayRadar;
    public bool displayShipIndicator;

    [Header("Start with powers")] 
    [SerializeField] private bool divineLightning;
    [SerializeField] private bool fairy;
    [SerializeField] private bool shield;
    [SerializeField] private bool ghost;
    [SerializeField] private bool blackHole;
    [SerializeField] private bool synthWave;
    [SerializeField] private bool toxicZone;
    [SerializeField] private bool dagger;

    [Header("Early Upgrades")]
    [SerializeField] bool startWithMinerBot;
    [SerializeField] private RectTransform debugLayout;
    [SerializeField] private TextMeshProUGUI debugLine;

    public static DebugManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (startWithResources) PlayerController.instance.debug_GiveResources(50);
        if (startWithMinerBot) PlayerController.instance.SpawnMinerBot();
        //if (noEnnemySpawn) SpawnManager.instance.debug_StopEnnemySpawn();
        //if (noTimer) Timer.instance.debug_StopTimer();
        
        if (divineLightning) AcquirePower(Vault.power.DivineLightning);
        if (fairy) AcquirePower(Vault.power.Fairy);
        if (shield) AcquirePower(Vault.power.Shield);
        if (ghost) AcquirePower(Vault.power.Ghost);
        if (blackHole) AcquirePower(Vault.power.BlackHole);
        if (synthWave) AcquirePower(Vault.power.SynthWave);
        if (toxicZone) AcquirePower(Vault.power.ToxicZone);
        if (dagger) AcquirePower(Vault.power.Dagger);

        areUpgradesFree = freeUpgrades;
    }

    public static void DisplayValue(string name, string value)
    {
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
        powerHandler.Activate();
        ObjectManager.HideAltarUI();
        ObjectManager.altar?.DepleteAltar();
    }

}
