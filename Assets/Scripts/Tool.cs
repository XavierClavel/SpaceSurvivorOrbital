using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool : MonoBehaviour
{
    List<Resource> resourcesInRange = new List<Resource>();
    [SerializeField] CircleCollider2D trigger;

    public void Resize(float range)
    {
        trigger.radius = range;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        resourcesInRange.Add(SpawnManager.dictObjectToResource[other.gameObject]);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        resourcesInRange.Remove(SpawnManager.dictObjectToResource[other.gameObject]);
    }

    public void Hit(int damage)
    {
        List<Resource> resourcesToHit = resourcesInRange.ToArray().ToList();
        foreach (Resource resource in resourcesToHit)
        {
            resource.Hit(damage);
        }
    }

}
