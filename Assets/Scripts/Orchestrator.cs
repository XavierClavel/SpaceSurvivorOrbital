using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] DataManager dataManager;
    public static MonoBehaviour context;
    // Start is called before the first frame update
    void Awake()
    {
        dataManager.LoadData();
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

    // Update is called once per frame
    void Update()
    {

    }
}
