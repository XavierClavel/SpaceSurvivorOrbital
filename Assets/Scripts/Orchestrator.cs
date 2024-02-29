using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] DataManager dataManager;
    [SerializeField] private DebugManager debugManager;
    public static MonoBehaviour context;

    private ShakeManager shakeManager;
    // Start is called before the first frame update
    void Awake()
    {
        shakeManager = gameObject.AddComponent<ShakeManager>();
        shakeManager.Setup();
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
