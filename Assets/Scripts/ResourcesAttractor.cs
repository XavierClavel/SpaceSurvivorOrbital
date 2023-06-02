using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesAttractor : MonoBehaviour
{
    [SerializeField] CircleCollider2D attractorZone;
    float attractorForce;

    private void Start()
    {
        attractorZone.radius = PlayerManager.attractorRange;
        attractorForce = PlayerManager.attractorForce;
        PlayerController.instance.attractorTransform = transform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3 distanceVector = transform.position - other.transform.position;
        other.transform.position += distanceVector.normalized * Time.fixedDeltaTime * attractorForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision");
        switch (other.gameObject.tag)
        {
            case "VioletCollectible":
                PlayerController.instance.IncreaseViolet();
                break;

            case "GreenCollectible":
                PlayerController.instance.IncreaseGreen();
                break;

            case "OrangeCollectible":
                PlayerController.instance.IncreaseOrange();
                break;
        }

        Destroy(other.gameObject);
    }
}
