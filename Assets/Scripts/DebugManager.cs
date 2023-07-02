using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] bool noEnnemySpawn;
    [SerializeField] bool noTimer;
    [SerializeField] bool startWithResources;
    [Header("Early Upgrades")]
    [SerializeField] bool startWithRadar;
    [SerializeField] bool startWithMinerBot;

    public static bool testVersion = true;
    // Start is called before the first frame update

    void Start()
    {
        if (startWithResources) PlayerController.instance.debug_GiveResources(50);
        if (startWithMinerBot) PlayerController.instance.SpawnMinerBot();
        if (startWithRadar) PlayerController.instance.debug_ActivateRadar();
        if (noEnnemySpawn) SpawnManager.instance.debug_StopEnnemySpawn();
        if (noTimer) Timer.instance.debug_StopTimer();
    }

}
