using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowModeSelector : ItemSelector
{
    public override void onSelected(string key)
    {
        if (key == "fullscreen")
        {
            Screen.fullScreen = true;
        } else if (key == "windowed")
        {
            Screen.fullScreen = false;
        }
        else
        {
            Debug.LogError($"Unrecognized key '{key}'");
        }
    }
}
