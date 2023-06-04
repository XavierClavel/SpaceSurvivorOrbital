using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeRemaining = 30;
    bool timerIsRunning = false;
    [SerializeField] TextMeshProUGUI timeText; // r�f�rence au composant Text de l'UI
    [SerializeField] float timeToAdd = 5; // temps � ajouter lorsqu'une touche est enfonc�e

    [SerializeField] GameObject boss;
    Transform playerTransform;
    GameObject spaceShip;

    void Start()
    {
        timerIsRunning = true;
        playerTransform = PlayerController.instance.transform;
        spaceShip = GameObject.FindGameObjectWithTag("Ship");
    }

    void Update()
    {
        if (!timerIsRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeText.text = "Temps restant : " + Mathf.RoundToInt(timeRemaining).ToString(); // affiche le temps restant dans l'UI
        }
        else
        {
            Instantiate(boss, randomPos() + playerTransform.position, Quaternion.identity);
            Destroy(spaceShip);
            timeRemaining = 0;
            timerIsRunning = false;
        }
    }

    InputMaster controls;

    void Awake()
    {
        controls = new InputMaster();
    }

    void OnDisable()
    {
        controls.Disable();
    }
        Vector3 randomPos()
    {
        float signA = Random.Range(0, 2) * 2 - 1;
        float signB = Random.Range(0, 2) * 2 - 1;
        return signA * Random.Range(6f, 12f) * Vector2.up + signB * Random.Range(4f, 8f) * Vector2.right;
    }

}
