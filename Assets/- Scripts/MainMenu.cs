using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    enum items {Play, Levels, Quit};
    Quaternion mainMenuPos = Quaternion.identity;
    Quaternion levelsScreenPos = Quaternion.Euler(0f, 90f, 0f);
    Transform camTransform;
    int levelReached;
    [NamedArray(typeof(items))] [SerializeField] GameObject[] menuItems;
    [HideInInspector] public InputMaster controls;
    GamepadMenuHandler menuHandler;

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    private void Awake() {
        camTransform = Camera.main.transform;
        levelReached = PlayerPrefs.GetInt("levelReached", 1);
        menuHandler = new GamepadMenuHandler(menuItems, false);

        controls = new InputMaster();
        controls.PauseMenu.NavigateDown.performed += context => menuHandler.NavigateDown();
        controls.PauseMenu.NavigateUp.performed += context => menuHandler.NavigateUp();
        controls.PauseMenu.Validate.performed += context => menuHandler.Validate();

        Debug.Log(Gamepad.all.Count);

        if (Gamepad.all.Count > 0) {
            menuItems[0].GetComponent<Animator>().SetBool("Highlighted", true);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Bossfight");
        /*try {
            SceneManager.LoadScene("Level Reference");
            //SceneManager.LoadScene("Level " + levelReached);
        }
        catch {
            SceneManager.LoadScene("Level Reference");
        }*/
    }

    public void ShowLevels()
    {
        StartCoroutine(MoveToLevelScreen());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator ToLevelsScreen()
    {   
        float totalTime = 0.1f;
        int nbPoints = 50;
        float step = 1f/nbPoints;
        float stepTime = totalTime*step;
        for (int i = 0; i <= nbPoints; i ++) {
            yield return new WaitForSeconds(stepTime);
            camTransform.rotation = Quaternion.Slerp(mainMenuPos, levelsScreenPos, i*step);
        }
    }

    public void BackToMainMenu()
    {
        StartCoroutine(ToMainMenu());
    }

    IEnumerator ToMainMenu()
    {   
        float totalTime = 0.1f;
        int nbPoints = 50;
        float step = 1f/nbPoints;
        float stepTime = totalTime*step;
        WaitForSeconds waitStep = Helpers.GetWait(stepTime);
        for (int i = 0; i <= nbPoints; i ++) {
            yield return waitStep;
            camTransform.rotation = Quaternion.Slerp(levelsScreenPos, mainMenuPos, i*step);
        }
    }

    IEnumerator MoveToLevelScreen()
    {
        float duration = 2f;
        float invDuration = 1f/duration;
        float startTime = Time.time;
        float ratio = 0f;
        WaitForEndOfFrame waitFrame = Helpers.GetWaitFrame;

        while (ratio < 1f) {
            ratio = (Time.time - startTime)*invDuration;
            camTransform.rotation = Quaternion.Slerp(levelsScreenPos, mainMenuPos, ratio);
            camTransform.position = Vector3.Slerp(Vector3.zero, new Vector3(100, 50, 200), ratio);
            yield return waitFrame;
        }
    }


}
