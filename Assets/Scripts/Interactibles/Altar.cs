using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject inputPrompt;

    private void Start()
    {
        SpawnManager.dictObjectToInteractable.Add(gameObject, this);
    }

    public void StartInteracting()
    {
        ObjectManager.DisplayAltarUI();
        Destroy(gameObject);
    }

    public void StopInteracting() { }
    public void Interacting() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        inputPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inputPrompt.SetActive(false);
    }
}
