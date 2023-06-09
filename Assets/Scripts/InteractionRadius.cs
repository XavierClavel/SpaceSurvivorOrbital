using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractionRadius : MonoBehaviour
{
    InputMaster controls;
    public static List<IInteractable> interactables = new List<IInteractable>();
    bool interacting = false;

    private void Start()
    {
        controls = new InputMaster();
        controls.Player.Mine.started += ctx =>
        {
            interacting = true;
            if (interactables.Count == 0) return;
            List<IInteractable> list = interactables.ToArray().ToList();
            foreach (IInteractable interactable in list)
            {
                interactable.StartInteracting();
            }
        };



        controls.Player.Mine.canceled += ctx =>
        {
            interacting = false;
            if (interactables.Count == 0) return;
            List<IInteractable> list = interactables.ToArray().ToList();
            foreach (IInteractable interactable in list)
            {
                interactable.StopInteracting();
            }
        };

        controls.Enable();
    }

    private void FixedUpdate()
    {
        if (interactables.Count == 0) return;
        if (!interacting) return;
        List<IInteractable> list = interactables.ToArray().ToList();
        foreach (IInteractable interactable in list)
        {
            interactable.Interacting();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        interactables.Add(ObjectManager.dictObjectToInteractable[other.gameObject]);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!ObjectManager.dictObjectToInteractable.ContainsKey(other.gameObject)) return;
        IInteractable interactable = ObjectManager.dictObjectToInteractable[other.gameObject];
        interactables.Remove(interactable);

    }

    private void OnDestroy()
    {
        interactables = new List<IInteractable>();
    }
}
