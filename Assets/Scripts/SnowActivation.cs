using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowActivation : MonoBehaviour
{
    public GameObject player; // R�f�rence vers le transform du joueur

    private void Start()
    {
        player = PlayerController.instance.gameObject;
    }
    private void Update()
    {
        // Suivre la position du joueur
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y +10);
    }
}
