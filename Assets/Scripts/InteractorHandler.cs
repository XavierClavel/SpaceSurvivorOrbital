using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorHandler : MonoBehaviour
{
    [SerializeField] protected Interactor weapon;
    [SerializeField] protected Interactor tool; //can be null;
    protected bool action = false;
    protected Interactor currentInteractor;
    int resourcesInRange = 0;

    private void Start()
    {
        currentInteractor = weapon;
    }

    void StartAction()
    {
        action = true;
        currentInteractor.StartUsing();
    }

    void StopAction()
    {
        action = false;
        currentInteractor.StopUsing();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (resourcesInRange++ == 0) SwitchInteractor();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (--resourcesInRange == 0) SwitchInteractor();
    }

    private void SwitchInteractor()
    {
        if (tool == null)
        {
            currentInteractor.SwitchMode();
        }
        else
        {
            currentInteractor.StopUsing();
            currentInteractor = currentInteractor == weapon ? tool : weapon;
            currentInteractor.StartUsing();
        }
    }

}
