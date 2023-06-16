using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Tool : MonoBehaviour
{
    [HideInInspector] public List<Resource> resourcesInRange = new List<Resource>();
    CapsuleCollider2D trigger;
    protected int toolPower;
    protected float toolReloadTime;

    private bool toolReloading = false;
    private bool mining = false;
    int layerMask;
    [SerializeField] LayerMask mask;
    [HideInInspector] public UnityEvent onEnterResourceRange = new UnityEvent();
    [HideInInspector] public UnityEvent onLeaveResourceRange = new UnityEvent();
    [HideInInspector] public UnityEvent onNoRessourcesLeft = new UnityEvent();
    [HideInInspector] public UnityEvent<GameObject> onResourceExit = new UnityEvent<GameObject>();

    public void Initialize(Vector2 toolRange, int toolPower, float toolReloadTime)
    {
        trigger = GetComponent<CapsuleCollider2D>();
        trigger.size = toolRange;
        this.toolPower = toolPower;
        this.toolReloadTime = toolReloadTime;
        layerMask = LayerMask.NameToLayer("ResourcesOnly");
        layerMask = mask.value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (resourcesInRange.Count == 0) onEnterResourceRange.Invoke();
        resourcesInRange.Add(ObjectManager.dictObjectToResource[other.gameObject]);
        if (!mining) return;
        if (toolReloading) return;
        Hit();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        resourcesInRange.Remove(ObjectManager.dictObjectToResource[other.gameObject]);
        onResourceExit.Invoke(other.gameObject);

        if (resourcesInRange.Count != 0) return;
        onLeaveResourceRange.Invoke();
        if (mining)
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
        RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, trigger.size, trigger.direction, transform.eulerAngles.z, Vector2.up, 0f, layerMask);
        if (!hit)
        {
            onNoRessourcesLeft.Invoke();
            return;
        }
        mining = true;
        if (!toolReloading) Hit();
    }

    public void StopMining()
    {
        mining = false;
    }


}
