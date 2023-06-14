using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject inputPrompt;

    public void Activate()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Awake()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }

    private void Start()
    {
        ObjectManager.dictObjectToInteractable.Add(gameObject, this);
        PlayerController.SetupSpaceship(this);
    }

    public void StartInteracting()
    {
        PlayerManager.SetControlMode(PlayerController.isPlayingWithGamepad);
        PlayerManager.currentTimer = Timer.timeRemaining;
        SceneManager.LoadScene("Ship");
    }

    public void Interacting()
    {

    }

    public void StopInteracting()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        inputPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inputPrompt.SetActive(false);
    }

}
