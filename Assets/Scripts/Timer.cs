using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText; // référence au composant Text de l'UI
    public float timeToAdd = 5; // temps à ajouter lorsqu'une touche est enfoncée

    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timeText.text = "Temps restant : " + Mathf.RoundToInt(timeRemaining).ToString(); // affiche le temps restant dans l'UI
            }
            else
            {
                Debug.Log("Temps écoulé !");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void OnClick()
    {
        timeRemaining += timeToAdd;
    }
}
