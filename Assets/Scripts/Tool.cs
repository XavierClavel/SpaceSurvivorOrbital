using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool : MonoBehaviour
{
    List<Resource> resourcesInRange = new List<Resource>();
    [SerializeField] CapsuleCollider2D trigger;
    public static int toolPower;
    protected float toolReloadTime;
    private bool toolReload;

    private void Start()
    {
        trigger.size = new Vector2(Helpers.FloorFloat(PlayerManager.toolRange, 2.5f), Helpers.FloorFloat(PlayerManager.toolRange, 3.5f));
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
            if (!toolReload)
            {
                resource.Hit(toolPower);
                StartCoroutine(ToolReload());
            }

        }
    }
    IEnumerator ToolReload()
    {
        toolReload = true;
        yield return new WaitForSeconds(toolReloadTime);
        toolReload = false;
    }

}
