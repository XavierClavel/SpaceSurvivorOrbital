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

    bool miningPurple = false;

    public void Initialize(Interactor weaponInteractor, Interactor toolInteractor, Transform rotationAxis, bool playerInteractor = false)
    {
        player = PlayerController.instance;
        if (playerInteractor) playerInteractorHandler = this;

        weapon = Instantiate(weaponInteractor, transform.position, Quaternion.identity);
        weapon.Setup(PlayerManager.weaponData, tool == null);
        weapon.playerInteractor = playerInteractor;
        if (playerInteractor) weapon.reloadSlider = ObjectManager.instance.reloadSlider;
        weapon.transform.SetParent(rotationAxis);
        weapon.transform.position = transform.position + 0.3f * Vector3.left;

        currentInteractor = weapon;

        if (toolInteractor == null) return;

        tool = Instantiate(toolInteractor, transform.position, Quaternion.identity);
        tool.Setup(PlayerManager.toolData);
        tool.playerInteractor = playerInteractor;

        tool.transform.SetParent(rotationAxis);
        tool.transform.position = transform.position + 0.3f * Vector3.left;


    }

    public void StartMiningPurple()
    {
        miningPurple = true;
        StopAction(false);
    }

    public void StopMiningPurple()
    {
        miningPurple = false;
        if (action) StartAction();
    }


    public void StartAction()
    {
        action = true;
        if (miningPurple) return;
        currentInteractor.StartUsing();
        player.setSpeed(currentInteractor.speedWhileAiming);
    }

    public void StopAction(bool actionFalse = true)
    {
        if (actionFalse) action = false;
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
