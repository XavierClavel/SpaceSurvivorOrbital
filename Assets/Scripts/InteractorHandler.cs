using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorHandler : MonoBehaviour
{
    protected Interactor weapon;
    protected Interactor tool; //can be null;
    [HideInInspector] public bool action = false;
    [HideInInspector] public Interactor currentInteractor;
    int resourcesInRange = 0;
    PlayerController player;
    public static InteractorHandler playerInteractorHandler;

    public void Initialize(Interactor weaponInteractor, Interactor toolInteractor, Transform rotationAxis, bool playerInteractor = false)
    {
        player = PlayerController.instance;
        if (playerInteractor) playerInteractorHandler = this;

        weapon = Instantiate(weaponInteractor, transform.position, Quaternion.identity);
        weapon.Setup(PlayerManager.weaponData);
        weapon.playerInteractor = playerInteractor;
        if (playerInteractor) weapon.reloadSlider = ObjectManager.instance.reloadSlider;
        weapon.transform.SetParent(rotationAxis);
        weapon.transform.position = transform.position + 0.3f * Vector3.left;

        currentInteractor = weapon;
    }


    public void StartAction()
    {
        action = true;
        currentInteractor.StartUsing();
        player.setSpeed(currentInteractor.stats.speedWhileAiming);
    }

    public void StopAction(bool cancelAction = true)
    {
        if (cancelAction) action = false;
        currentInteractor.StopUsing();
        player.setSpeed(1f);
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
        if (tool == null) return;
        currentInteractor.SwitchMode();
        currentInteractor.StopUsing();
        currentInteractor.Switch(weapon, tool);
        currentInteractor.StartUsing();
    }

}
