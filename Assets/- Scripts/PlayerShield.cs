using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("BlueBullet") || other.gameObject.CompareTag("OrangeBullet")) {
            if (!player.hasBullet) {
                player.hasBullet = true; 
                GameManagement.instance.GlowCrosshair();
            }
        }
    }

}
