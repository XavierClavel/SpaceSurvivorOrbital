using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.ShaderGraph;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeRemaining = 30;
    [SerializeField] bool timerIsRunning = false;
    [SerializeField] TextMeshProUGUI timeText; // r�f�rence au composant Text de l'UI
    [SerializeField] float timeToAdd = 5; // temps � ajouter lorsqu'une touche est enfonc�e
    [SerializeField] GameObject youLooseScreen;
    EventSystem eventSystem;
    [SerializeField] GameObject button;

    void Start()
    {
        timerIsRunning = true;
        eventSystem = EventSystem.current;
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
            youLooseScreen.SetActive(true);
            PauseMenu.instance.PauseGame();
            if (PlayerController.isPlayingWithGamepad) eventSystem.SetSelectedGameObject(button);
            timeRemaining = 0;
            timerIsRunning = false;
        }
    }

    public void OnClick()
    {
        PauseMenu.instance.ResumeGame();
        SceneManager.LoadScene("SampleScene");
        youLooseScreen.SetActive(false);
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

}
