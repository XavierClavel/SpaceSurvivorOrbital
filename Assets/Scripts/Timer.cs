using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.ShaderGraph;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText; // r�f�rence au composant Text de l'UI
    public float timeToAdd = 5; // temps � ajouter lorsqu'une touche est enfonc�e
    public GameObject youLooseScreen;

    void Start()
    {
        timerIsRunning = true;
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
            PauseGame();
            timeRemaining = 0;
            timerIsRunning = false;
        }
    }

    public void OnClick()
    {
        ResumeGame();
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

    public void PauseGame()
    {
        if (!PlayerController.isPlayingWithGamepad) Cursor.visible = true;
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        PlayerController.instance.controls.Disable();
        controls.Enable();

        SoundManager.instance.StopTime();

    }
    public void ResumeGame()
    {
        if (!PlayerController.isPlayingWithGamepad) Cursor.visible = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        
        controls.PauseMenu.Disable();
        PlayerController.instance.controls.Enable();

        SoundManager.instance.ResumeTime();
    }

}
