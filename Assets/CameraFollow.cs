using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // La cible à suivre (par exemple, le joueur)
    public float smoothSpeed = 0.125f; // La vitesse de suivi
    public float lookaheadTime = 1.0f; // Le temps d'anticipation (lookahead time)
    private Vector2 offset;

    private void Start()
    {
        offset = (Vector2)transform.position - (Vector2)target.position;
    }

    private void FixedUpdate()
    {
        // Calculez la position de la caméra en utilisant des Vector2
        Vector2 targetAheadPosition = (Vector2)target.position + offset + (offset.normalized * lookaheadTime);

        // Convertissez la position résultante en Vector3 pour la caméra
        Vector3 targetAheadPosition3D = new Vector3(targetAheadPosition.x, targetAheadPosition.y, transform.position.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetAheadPosition3D, smoothSpeed);
        transform.position = smoothedPosition;
    }

}
