using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesAttractor : MonoBehaviour
{
    [SerializeField] CircleCollider2D attractorZone;
    float attractorForce;

    private void Start()
    {
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
        switch (other.gameObject.tag)
        {
            case Vault.tag.PurpleCollectible:
                PlayerController.instance.IncreaseViolet();
                break;

            case Vault.tag.GreenCollectible:
                PlayerController.instance.IncreaseGreen();
                break;

            case Vault.tag.OrangeCollectible:
                PlayerController.instance.IncreaseOrange();
                break;
        }

        Destroy(other.gameObject);
    }
}
