using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    enum items {Resume, MainMenu, Quit};
    [SerializeField] GameObject pauseMenu;
    public static PauseMenu instance;
    public static bool canGameBePaused = true;
    [NamedArray(typeof(items))] [SerializeField] GameObject[] menuItems;
    [HideInInspector] public InputMaster controls;
    [SerializeField] GameObject talkPanel;
    bool talkPanelActive;
    GamepadMenuHandler menuHandler;

    void Awake()
    {
        instance = this;
        menuHandler = new GamepadMenuHandler(menuItems, true);

        controls = new InputMaster();
        controls.PauseMenu.NavigateDown.performed += context => menuHandler.NavigateDown();
        controls.PauseMenu.NavigateUp.performed += context => menuHandler.NavigateUp();
        controls.PauseMenu.Validate.performed += context => menuHandler.Validate();
        controls.PauseMenu.Pause.performed += context => ResumeGame();
        controls.Disable();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        pauseMenu.SetActive(true);

        talkPanelActive = talkPanel.activeInHierarchy;
        if (talkPanelActive) talkPanel.SetActive(false);

        PlayerController.instance.controls.Disable();
        controls.Enable();

        menuHandler.Reset();
        SoundManager.instance.StopTime();

        if (PlayerController.instance.controlScheme == PlayerController.controlMode.Keyboard) {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        pauseMenu.SetActive(false);

        if (talkPanelActive) talkPanel.SetActive(true);
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
