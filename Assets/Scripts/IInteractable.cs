using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public abstract void StartInteracting();
    public abstract void Interacting();
    public abstract void StopInteracting();

}
