using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRadius : MonoBehaviour
{
    InputMaster controls;
    List<IInteractable> interactables = new List<IInteractable>();
    bool interacting = false;

    private void Start()
    {
        controls = new InputMaster();
        controls.Player.Mine.started += ctx =>
        {
            interacting = true;
            foreach (IInteractable interactable in interactables)
            {
                interactable.StartInteracting();
            }
        };



        controls.Player.Mine.canceled += ctx =>
        {
            interacting = false;
            foreach (IInteractable interactable in interactables)
            {
                interactable.StopInteracting();
            }
        };

        controls.Enable();
    }

    private void FixedUpdate()
    {

        if (!interacting) return;
        foreach (IInteractable interactable in interactables)
        {
            interactable.Interacting();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        interactables.Add(SpawnManager.dictObjectToInteractable[other.gameObject]);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = SpawnManager.dictObjectToInteractable[other.gameObject];
        interactables.Remove(interactable);
        if (!interactable.TryRemove()) return;
        SpawnManager.dictObjectToInteractable.Remove(other.gameObject);
        Destroy(other.gameObject);

    }
}
