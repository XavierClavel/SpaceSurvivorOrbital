using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerFlame : Power
{
    void Update()
    {
        gameObject.transform.position = player.transform.position;
        // Obtenir la direction de visée depuis le PlayerController
        Vector2 aimDirection = player.aimVector;

        if (aimDirection != Vector2.zero)
        {
            // Calculer l'angle en radians
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

            // Appliquer la rotation
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
