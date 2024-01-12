using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] DataManager dataManager;
    [SerializeField] private DebugManager debugManager;
    public static MonoBehaviour context;
    // Start is called before the first frame update
    void Awake()
    {
        dataManager.LoadData();
        debugManager.LoadData();
        context = this;
    }

    private void OnDestroy()
    {
        Reset();
    }

    void Reset()
    {
        PoolManager.reset();
    }

}
