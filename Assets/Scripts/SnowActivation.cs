using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowActivation : MonoBehaviour
{
    public GameObject player; // Référence vers le transform du joueur
    public int yModifier;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {

        if (player != null)     
        {
            // Suivre la position du joueur
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y +yModifier);
        }
    }
}
