using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static PauseMenu instance;
    public static bool canGameBePaused = true;
    InputMaster controls;

    void Awake()
    {
        instance = this;
        controls = new InputMaster();
        controls.Player.Pause.performed += ctx => ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        pauseMenu.SetActive(true);

        PlayerController.instance.controls.Disable();
        controls.Enable();

        SoundManager.instance.StopTime();

        if (PlayerController.instance.controlScheme == PlayerController.controlMode.Keyboard)
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        controls.PauseMenu.Disable();
        PlayerController.instance.controls.Enable();

        SoundManager.instance.ResumeTime();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
