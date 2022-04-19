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
    [HideInInspector] public bool gamePaused = false;
    public static PauseMenu instance;
    public static bool canGameBePaused = true;
    [NamedArray(typeof(items))] [SerializeField] GameObject[] menuItems;
    [HideInInspector] public InputMaster controls;
    int index = 0;
    int lastIndex = 0;
    [SerializeField] GameObject talkPanel;
    bool talkPanelActive;

    void Awake()
    {
        instance = this;
        controls = new InputMaster();
        controls.PauseMenu.NavigateDown.performed += context => NavigateDown();
        controls.PauseMenu.NavigateUp.performed += context => NavigateUp();
        controls.PauseMenu.Validate.performed += context => Validate();
        controls.Disable();
    }

    public void Pause()
    {
        if (gamePaused) {
            ResumeGame();
        }
        else {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        pauseMenu.SetActive(true);
        gamePaused = true;
        instance.controls.Disable();
        if (PlayerController.instance.controlScheme == PlayerController.controlMode.Keyboard) Cursor.lockState = CursorLockMode.None;
        index = 0;
        lastIndex = 0;
        controls.Enable();
        talkPanelActive = talkPanel.activeInHierarchy;
        if (talkPanelActive) talkPanel.SetActive(false);

        SoundManager.instance.StopTime();

        if (PlayerController.instance.controlScheme == PlayerController.controlMode.Gamepad) {
            menuItems[index].GetComponent<Animator>().SetBool("Highlighted", true);
        }
        
    }

    void NavigateDown()
    {
        index ++;
        if (index == menuItems.Length) index = 0;
        Animate(ref lastIndex, index);
    }

    void NavigateUp()
    {
        index --;
        if (index < 0) index = menuItems.Length - 1;
        Animate(ref lastIndex, index);
        
    }

    void Animate(ref int lastIndex, int newIndex)
    {
        menuItems[lastIndex].GetComponent<Animator>().SetBool("Normal", true);
        menuItems[newIndex].GetComponent<Animator>().SetBool("Highlighted", true);
        lastIndex = newIndex;
    }

    void Validate()
    {
        menuItems[index].GetComponent<Button>().onClick.Invoke();
    }

    public void ResumeGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        pauseMenu.SetActive(false);
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        controls.PauseMenu.Disable();
        if (talkPanelActive) talkPanel.SetActive(true);

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
