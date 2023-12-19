using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Power
{
    public string tagEnnemy = "Ennemy";
    public float forceAttraction = 10f;

    void OnTriggerStay(Collider other)
    {
        // Vérifier si l'objet a le tag cible
        if (other.CompareTag(tagEnnemy))
        {
            Attraction(other.GetComponent<Rigidbody>());
        }
    }

    void Attraction(Rigidbody objetRB)
    {
        Vector3 direction = transform.position - objetRB.position;
        float distance = direction.magnitude;

        float force = forceAttraction / distance;

        objetRB.AddForce(direction.normalized * force);
    }
}
