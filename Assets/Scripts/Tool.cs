using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Tool : MonoBehaviour
{
    [HideInInspector] public List<Resource> resourcesInRange = new List<Resource>();
    [SerializeField] CapsuleCollider2D trigger;
    protected int toolPower;
    protected float toolReloadTime;

    private bool toolReloading = false;
    private bool mining = false;

    [HideInInspector] public UnityEvent onNoRessourcesLeft = new UnityEvent();
    [HideInInspector] public UnityEvent<GameObject> onResourceExit = new UnityEvent<GameObject>();

    public void Initialize(Vector2 toolRange, int toolPower, float toolReloadTime)
    {
        trigger.size = toolRange;
        this.toolPower = toolPower;
        this.toolReloadTime = toolReloadTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("tool trigger");
        resourcesInRange.Add(SpawnManager.dictObjectToResource[other.gameObject]);
        if (!mining) return;
        if (toolReloading) return;
        Hit();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        resourcesInRange.Remove(SpawnManager.dictObjectToResource[other.gameObject]);
        onResourceExit.Invoke(other.gameObject);

        if (mining && resourcesInRange.Count == 0)
        {
            mining = false;
            onNoRessourcesLeft.Invoke();

        }
    }


    public void Hit()
    {
        List<Resource> resourcesToHit = resourcesInRange.ToArray().ToList();
        foreach (Resource resource in resourcesToHit)
        {
            resource.Hit(toolPower);
        }
        StartCoroutine(nameof(ToolReload));
    }
    IEnumerator ToolReload()
    {
        toolReloading = true;
        yield return new WaitForSeconds(toolReloadTime);
        toolReloading = false;
        if (mining) Hit();
    }

    public void StartMining()
    {
        Debug.Log(resourcesInRange.Count);
        mining = true;
        if (resourcesInRange.Count == 0) return;
        if (!toolReloading) Hit();
    }

    public void StopMining()
    {
        mining = false;
    }


}
