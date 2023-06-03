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

    void OnDisable()
    {
        controls.Disable();
    }

    public void PauseGame(bool pauseUI = true)
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        SoundManager.instance.StopTime();

        PlayerController.instance.controls.Disable();
        if (!PlayerController.isPlayingWithGamepad) Cursor.visible = true;

        if (pauseUI)
        {
            pauseMenu.SetActive(true);
            controls.Enable();
        }

    }

    public void ResumeGame()
    {

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        pauseMenu.SetActive(false);


        controls.PauseMenu.Disable();
        PlayerController.instance.controls.Enable();
        PlayerController.SwitchInput();

        SoundManager.instance.ResumeTime();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
