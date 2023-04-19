using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour, IInteractable
{

    private void Start()
    {
        Planet.dictObjectToInteractable.Add(gameObject, this);
    }

    public void StartInteracting()
    {
        SceneManager.LoadScene("Ship");
    }

    public void Interacting()
    {

    }

    public void StopInteracting()
    {

    }
}
