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
        PauseMenu.instance.ResumeGame();
        SceneManager.LoadScene(text);
    }
}
