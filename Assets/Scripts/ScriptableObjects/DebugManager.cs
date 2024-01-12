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
    [SerializeField] private bool shield;
    [SerializeField] private bool ghost;
    [SerializeField] private bool blackHole;
    [SerializeField] private bool synthWave;
    [SerializeField] private bool toxicZone;
    [SerializeField] private bool dagger;

    private RectTransform debugLayout;
    private TextMeshProUGUI debugLine;

    private static DebugManager instance;

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

    public void Setup()
    {
        instance = this;
    }

    public void LoadData()
    {
        if (!debugEnabled) return;
        
        if (startWithResources) PlayerController.instance.debug_GiveResources(50);
        
        if (divineLightning) AcquirePower(Vault.power.DivineLightning);
        if (fairy) AcquirePower(Vault.power.Fairy);
        if (shield) AcquirePower(Vault.power.Shield);
        if (ghost) AcquirePower(Vault.power.Ghost);
        if (blackHole) AcquirePower(Vault.power.BlackHole);
        if (synthWave) AcquirePower(Vault.power.SynthWave);
        if (toxicZone) AcquirePower(Vault.power.ToxicZone);
        if (dagger) AcquirePower(Vault.power.Dagger);

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

}
