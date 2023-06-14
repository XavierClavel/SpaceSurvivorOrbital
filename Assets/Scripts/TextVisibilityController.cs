using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVisibilityController : MonoBehaviour
{
    public GameObject textObject;
    public GameObject monster;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(true);
            monster.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(false);
        }
    }
}
