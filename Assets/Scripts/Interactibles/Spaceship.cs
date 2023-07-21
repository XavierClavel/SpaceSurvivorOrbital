using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spaceship : MonoBehaviour, IInteractable
{
    [SerializeField] Image image;
    [SerializeField] float timeToLaunch = 10f;
    float factor;
    float fillAmount = 1;
    bool hasExitedRadius = false;
    [SerializeField] new Collider2D collider;
    static Spaceship instance;

    public void Activate()
    {
        ObjectManager.spaceshipIndicator.gameObject.SetActive(true);
        collider.enabled = true;
    }

    public void Deactivate()
    {
        ObjectManager.spaceshipIndicator.gameObject.SetActive(false);
        collider.enabled = false;
    }

    public static void UpdateSpaceship()
    {
        if (PlayerManager.amountPurple > 0) instance.Activate();
        else instance.Deactivate();
    }

    private void Awake()
    {
        instance = this;
        collider = GetComponent<Collider2D>();
        collider.enabled = false;

    }

    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        factor = 1f / timeToLaunch;

        ObjectManager.spaceshipIndicator.target = transform;

        UpdateSpaceship();
        if (PlayerManager.amountGreen == 0) hasExitedRadius = true;
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
        Debug.Log(hasExitedRadius);
    }

    IEnumerator PrepareLaunch()
    {
        while (fillAmount > 0)
        {
            yield return Helpers.GetWaitFixed;
            fillAmount -= Time.fixedDeltaTime * factor;
            image.fillAmount = fillAmount;
        }

        LaunchShip();
    }

    void LaunchShip()
    {
        PlayerManager.SetControlMode(PlayerController.isPlayingWithGamepad);
        PlayerManager.currentTimer = Timer.timeRemaining;
        SceneManager.LoadScene(Vault.scene.Ship);
    }


}
