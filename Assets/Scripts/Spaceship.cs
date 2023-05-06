using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject inputPrompt;

    private void Start()
    {
        SpawnManager.dictObjectToInteractable.Add(gameObject, this);
    }

    public void StartInteracting()
    {
        PlayerManager.SetControlMode(PlayerController.isPlayingWithGamepad);
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

    public bool TryRemove()
    {
        return false;
    }
}
