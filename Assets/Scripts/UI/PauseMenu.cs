using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject firstSelected;

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

    public static void Pause()
    {
        instance.PauseGame();
    }

    public void PauseGame(bool pauseUI = true)
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        SoundManager.instance.StopTime();

        if (!Helpers.isPlatformAndroid()) PlayerController.instance.controls.Disable();
        if (!PlayerController.isPlayingWithGamepad && !Helpers.isPlatformAndroid()) Cursor.visible = true;

        if (pauseUI)
        {
            pauseMenu.SetActive(true);
            controls.Enable();
            InputManager.setSelectedObject(firstSelected);
        }

    }

    public static void ResumeTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void ResumeGame()
    {
        ResumeTime();

        pauseMenu?.SetActive(false);


        controls.PauseMenu.Disable();
        if (!Helpers.isPlatformAndroid()) PlayerController.instance.controls.Enable();
        InputManager.setSelectedObject(null);

        SoundManager.instance.ResumeTime();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneTransitionManager.TransitionToScene(gameScene.titleScreen);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
