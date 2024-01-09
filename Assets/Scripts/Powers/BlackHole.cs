using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Power
{
    public string tagEnnemy = "Ennemy";
    public float forceAttraction = 0.1f;

    void OnTriggerStay2D(Collider2D other)
    {
        // Vérifier si l'objet a le tag cible
        if (other.CompareTag(tagEnnemy))
        {
            Attraction(other.GetComponent<Rigidbody2D>());
        }
    }

    void Attraction(Rigidbody2D rb)
    {
        Vector3 direction = -rb.position;
        float distance = direction.magnitude;

        float force = forceAttraction / distance;

        rb.AddForce(direction.normalized * force);
    }
}
