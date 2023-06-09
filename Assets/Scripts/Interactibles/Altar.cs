using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
    private void Start()
    {
        SpawnManager.dictObjectToInteractable.Add(gameObject, this);
    }
    public void StartInteracting()
    {
        PlayerController.instance.SpawnMinerBot();
    }

    public void StopInteracting() { }
    public void Interacting() { }
}
