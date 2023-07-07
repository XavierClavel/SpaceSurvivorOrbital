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

    public void Activate()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Awake()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        PlayerController.SetupSpaceship(this);
    }

    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        factor = 1f / timeToLaunch;
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
