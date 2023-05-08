using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour
{
    public string text;

    public void OnClick()
    {
        SceneManager.LoadScene(text);
    }
}
