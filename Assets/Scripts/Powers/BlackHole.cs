using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Power
{
    public float forceAttraction = 0.1f;

    void OnTriggerStay2D(Collider2D other)
    {
        // V�rifier si l'objet a le tag cible
        if (other.CompareTag(Vault.tag.Ennemy))
        {
            Ennemy ennemy = ObjectManager.dictObjectToEnnemy[other.gameObject];
            ennemy.ApplyForce(CalculateForce(ennemy.transform));
        }
    }

    private Vector2 CalculateForce(Transform t)
    {
        Vector3 direction = transform.position - t.position;
        float force = forceAttraction / direction.sqrMagnitude;
        return direction * force;
    }
}
