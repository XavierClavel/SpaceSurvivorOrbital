using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    public string text;

    public void OnClickShip()
    {
        SceneManager.LoadScene(text);
    }

    public void OnClickPlanet()
    {
        PlayerManager.SpendPurple(1);
        SceneManager.LoadScene(text);
    }

    public void OnclickWin()
    {
        SceneManager.LoadScene(text);

        Destroy(GameObject.FindGameObjectWithTag("Win"));

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        PlayerController.instance.controls.Enable();
        InputManager.setSelectedObject(null);

        SoundManager.instance.ResumeTime();
        Destroy(this);
    }
}
