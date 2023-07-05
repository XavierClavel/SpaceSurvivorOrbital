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
        weapon.Setup(PlayerManager.weaponStats);
        weapon.playerInteractor = playerInteractor;
        if (playerInteractor) weapon.reloadSlider = ObjectManager.instance.reloadSlider;
        weapon.transform.SetParent(rotationAxis);
        weapon.transform.position = transform.position + 0.3f * Vector3.left;

        if (toolInteractor == null)
        {
            weapon.currentLayerMask = LayerMask.GetMask("Resources", "Ennemies");
        }
        else
        {
            /*
            tool = Instantiate(PlayerManager.weapon, transform.position, Quaternion.identity);
            tool.reloadSlider = ObjectManager.instance.reloadSlider;
            tool.transform.SetParent(ObjectManager.instance.armTransform);
            tool.transform.position = transform.position + 0.3f * Vector3.left;
            tool.Setup(PlayerManager.toolStats);

            weapon.currentLayerMask = LayerMask.GetMask("EnnemiesOnly");
            tool.currentLayerMask = LayerMask.GetMask("ResourcesOnly");
            //Instantiate(PlayerManager.tool, transform.position, Quaternion.identity);
            //tool.transform.SetParent(transform);
            //tool.Initialize(new Vector2(PlayerManager.toolRange, PlayerManager.toolRange), PlayerManager.toolPower, PlayerManager.toolReloadTime);
            //TODO? Vector2 for toolRange in PlayerManager
            */
        }

        currentInteractor = weapon;
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
