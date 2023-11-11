using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] bool noEnnemySpawn;
    [SerializeField] bool noTimer;
    [SerializeField] bool startWithResources;
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
    }

    public static void DisplayValue(string name, string value)
    {
        if (instance.debugLayout == null) return;
        var obj = GameObject.Instantiate(instance.debugLine);
        obj.transform.SetParent(instance.debugLayout);
        obj.SetText($"{name}: {value}");
    }

}
