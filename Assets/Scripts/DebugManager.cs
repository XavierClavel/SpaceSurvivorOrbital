using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static bool areUpgradesFree { get; private set; }
    
    
    [SerializeField] bool noEnnemySpawn;
    [SerializeField] bool noTimer;
    [SerializeField] bool startWithResources;
    [SerializeField] private bool freeUpgrades;

    [Header("Start with powers")] 
    [SerializeField] private bool divineLightning;
    [SerializeField] private bool fairy;
    [SerializeField] private bool shield;
    [SerializeField] private bool ghost;
    [SerializeField] private bool blackHole;
    [SerializeField] private bool synthWave;

    [Header("Early Upgrades")]
    [SerializeField] bool startWithRadar;
    [SerializeField] bool startWithMinerBot;
    [SerializeField] private RectTransform debugLayout;
    [SerializeField] private TextMeshProUGUI debugLine;

    public static bool testVersion = true;
    // Start is called before the first frame update
    private static DebugManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (startWithResources) PlayerController.instance.debug_GiveResources(50);
        if (startWithMinerBot) PlayerController.instance.SpawnMinerBot();
        if (startWithRadar) PlayerController.instance.debug_ActivateRadar();
        //if (noEnnemySpawn) SpawnManager.instance.debug_StopEnnemySpawn();
        //if (noTimer) Timer.instance.debug_StopTimer();
        
        if (divineLightning) AcquirePower(Vault.power.DivineLightning);
        if (fairy) AcquirePower(Vault.power.Fairy);
        if (shield) AcquirePower(Vault.power.Shield);
        if (ghost) AcquirePower(Vault.power.Ghost);
        if (blackHole) AcquirePower(Vault.power.BlackHole);
        if (synthWave) AcquirePower(Vault.power.SynthWave);

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
        ObjectManager.altar.DepleteAltar();
    }

}
