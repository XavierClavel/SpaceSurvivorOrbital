using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool : MonoBehaviour
{
    List<Resource> resourcesInRange = new List<Resource>();
    [SerializeField] CapsuleCollider2D trigger;
    protected int toolPower;
    protected float toolReloadTime;

    private bool toolReloading = false;
    private bool mining = false;

    public void Initialize(Vector2 toolRange, int toolPower, float toolReloadTime)
    {
        trigger.size = toolRange;
        this.toolPower = toolPower;
        this.toolReloadTime = toolReloadTime;
    }

    //TODO : replace with initialize method
    private void Start()
    {
        trigger.size = new Vector2(Helpers.FloorFloat(PlayerManager.toolRange, 3.5f), Helpers.FloorFloat(PlayerManager.toolRange, 3.5f));
        toolPower = PlayerManager.toolPower;
        toolReloadTime = PlayerManager.toolReloadTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        resourcesInRange.Add(SpawnManager.dictObjectToResource[other.gameObject]);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        resourcesInRange.Remove(SpawnManager.dictObjectToResource[other.gameObject]);
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
        mining = true;
        if (!toolReloading) Hit();
    }

    public void StopMining()
    {
        mining = false;
    }


}
