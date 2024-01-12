using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spaceship : MonoBehaviour, IInteractable
{
    [SerializeField] Image image;
    [SerializeField] float timeToLaunch = 10f;
    [SerializeField] ParticleSystem launchEffect;
    float factor;
    float fillAmount = 1;
    bool hasExitedRadius = true;
    static Spaceship instance;


    private void Awake()
    {
        instance = this;
        ObjectManager.spaceshipIndicator.gameObject.SetActive(true);
    }

    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        factor = 1f / timeToLaunch;

        ObjectManager.spaceshipIndicator.target = transform;

        ObjectManager.spaceship = gameObject;
        if (DebugManager.instance.shipInstantTP) fillAmount = 0f;
        if (!DebugManager.instance.shipPresent) ObjectManager.HideSpaceship();
        
    }

    public void StartInteracting()
    {

    }

    public void Interacting()
    {

    }

    public void StopInteracting()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasExitedRadius) return;
        image.gameObject.SetActive(true);
        StartCoroutine(nameof(PrepareLaunch));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(nameof(PrepareLaunch));
        fillAmount = 1;
        image.fillAmount = fillAmount;
        image.gameObject.SetActive(false);

        hasExitedRadius = true;
    }

    IEnumerator PrepareLaunch()
    {
        while (fillAmount > 0)
        {
            yield return Helpers.GetWaitFixed;
            Helpers.SpawnPS(transform, launchEffect);
            fillAmount -= Time.fixedDeltaTime * factor;
            image.fillAmount = fillAmount;
            SoundManager.PlaySfx(transform, key: "Ship_Loading");
        }

        LaunchShip();
    }

    void LaunchShip()
    {
        SoundManager.PlaySfx(transform, key: "Ship_TakeOff");
        PlayerManager.SetControlMode(PlayerController.isPlayingWithGamepad);
        //PlayerManager.currentTimer = Timer.timeRemaining;
        PlayerManager.setCurrentHealth(PlayerController.instance.health);
        PlayerManager.setSouls(PlayerController.instance.souls);
        
        SaveManager.Save();
        
        SceneTransitionManager.TransitionToScene(gameScene.ship);
    }


}
