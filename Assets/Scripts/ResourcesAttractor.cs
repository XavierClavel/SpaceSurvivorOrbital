using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class ResourcesAttractor : MonoBehaviour
{
    [SerializeField] CircleCollider2D attractorZone;
    float attractorForce;
    private static ResourcesAttractor instance;

    private void Start()
    {
        instance = this;
        attractorZone.radius = PlayerManager.playerData.attractor.range;
        attractorForce = PlayerManager.playerData.attractor.force;
        PlayerController.instance.attractorTransform = transform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3 distanceVector = transform.position - other.transform.position;
        other.transform.position += distanceVector.normalized * Time.fixedDeltaTime * attractorForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SoundManager.PlaySfx(transform, key: "Collectible_Yellow_Green");

        switch (other.gameObject.tag)
        {
            case Vault.tag.GreenCollectible:
                PlayerController.instance.IncreaseGreen();
                ObjectManager.recallItemGreen(other.gameObject);
                PlayerEventsManager.onResourcePickup(resourceType.green);
                break;

            case Vault.tag.OrangeCollectible:
                PlayerController.instance.IncreaseOrange();
                ObjectManager.recallItemOrange(other.gameObject);
                PlayerEventsManager.onResourcePickup(resourceType.orange);
                break;
        }
    }

    public static void ForceUpdate()
    {
        instance.attractorZone.enabled = false;
        instance.attractorZone.enabled = true;
    }

}
