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
    private bool isGamePaused = false;
    InputMaster controls;

    void Awake()
    {
        instance = this;
        controls = new InputMaster();
        controls.Player.Pause.performed += ctx => PauseUnpause();
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    private void PauseUnpause()
    {
        if (isGamePaused) ResumeGame();
        else PauseGame();
    }

    public static void Pause()
    {
        instance.PauseGame();
    }

    public void PauseGame(bool pauseUI = true)
    {
        isGamePaused = true;
        
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        Debug.Log(Time.timeScale);
        //SoundManager.instance.StopTime();

        if (!Helpers.isPlatformAndroid() && SceneManager.GetActiveScene().name == Vault.scene.Planet) PlayerController.instance.controls.Disable();
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
        isGamePaused = false;
            
        ResumeTime();

        pauseMenu?.SetActive(false);

        if (!Helpers.isPlatformAndroid() && SceneManager.GetActiveScene().name == Vault.scene.Planet) PlayerController.instance.controls.Enable();
        InputManager.setSelectedObject(null);

       // SoundManager.instance.ResumeTime();
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
