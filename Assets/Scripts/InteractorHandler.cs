using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorHandler : MonoBehaviour
{
    protected Interactor weapon;
    protected Interactor tool; //can be null;
    [HideInInspector] public bool action = false;
    protected Interactor currentInteractor;
    int resourcesInRange = 0;
    PlayerController player;

    private void Start()
    {

        player = PlayerController.instance;

        weapon = Instantiate(PlayerManager.weapon, transform.position, Quaternion.identity);
        weapon.reloadSlider = ObjectManager.instance.reloadSlider;
        weapon.transform.SetParent(ObjectManager.instance.armTransform);
        weapon.transform.position = transform.position + 0.3f * Vector3.left;

        if (tool == null)
        {
            weapon.currentLayerMask = LayerMask.GetMask("Resources", "Ennemies");
        }
        else
        {
            tool = Instantiate(PlayerManager.weapon, transform.position, Quaternion.identity);
            tool.reloadSlider = ObjectManager.instance.reloadSlider;
            tool.transform.SetParent(ObjectManager.instance.armTransform);
            tool.transform.position = transform.position + 0.3f * Vector3.left;

            weapon.currentLayerMask = LayerMask.GetMask("EnnemiesOnly");
            tool.currentLayerMask = LayerMask.GetMask("ResourcesOnly");
            //Instantiate(PlayerManager.tool, transform.position, Quaternion.identity);
            //tool.transform.SetParent(transform);
            //tool.Initialize(new Vector2(PlayerManager.toolRange, PlayerManager.toolRange), PlayerManager.toolPower, PlayerManager.toolReloadTime);
            //TODO? Vector2 for toolRange in PlayerManager
        }

        currentInteractor = weapon;
    }


    public void StartAction()
    {
        action = true;
        currentInteractor.StartUsing();
        player.setSpeed(currentInteractor.speedWhileAiming);
    }

    public void StopAction()
    {
        action = false;
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
